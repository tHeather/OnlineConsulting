const sendButton = document.getElementById("chat-send-button");
const messageInput = document.getElementById("chat-message-input");
const messageList = document.getElementById("chat-messages-list");
const chatIconButton = document.getElementById("chat-icon");
const chatContent = document.getElementById("chat-content");
const CONNECTION_ID_STORAGE_NAME = "conversationId";

/* WIDGET */
const urlSearchParams = new URLSearchParams(window.location.search);
const { client, sub } = Object.fromEntries(urlSearchParams.entries());

const handleClick = () => {
  document
    .getElementById("chat-content")
    .classList.toggle("chat-content--open");
  window.parent.postMessage("TOGGLE", client);
};

chatIconButton.addEventListener("click", handleClick);

/* FORM */
const textAreaAdjust = () => {
    messageInput.style.height = "1px";
    let scroll =
        messageInput.scrollHeight > 100 ? 100 : messageInput.scrollHeight;
    messageInput.style.height = `${scroll}px`;
    messageInput.style.overflowY = scroll === 100 ? "scroll" : "hidden";
};

messageInput.addEventListener("keyup", textAreaAdjust);

/* SIGNALR */
let conversationId = sessionStorage.getItem(CONNECTION_ID_STORAGE_NAME);
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/chatHub")
  .withAutomaticReconnect()
   .build();

const makeMessageTemplate = ({ createDate, content, isFromClient }) => {
    const li = document.createElement("li");
    const messageTemplate = `
        <div class="chat-messages-list__content" >${content}</div>
        <div class="chat-messages-list__date">
        ${dayjs.utc(createDate, "DD-MM-YYYY HH:mm").local().format("HH:mm")}
        </div>
        `;

     li.innerHTML = messageTemplate;
     li.classList.add("chat-messages-list__message");
     if (isFromClient) li.classList.add("chat-messages-list__message--client");
    return li;
}

const appendMessage = (message) => {

    const messageTemplate = makeMessageTemplate(message);

    if (messageList.lastChild) {
        messageList.lastChild.after(messageTemplate);
    } else {
        messageList.appendChild(messageTemplate);
    }

    messageList.scrollTo(0, messageList.scrollHeight);

};

const disableControls = (isDisabled) => {
    messageInput.disabled = isDisabled;
    sendButton.disabled = isDisabled;
};

const clearInput = () => {
    messageInput.value = "";
    textAreaAdjust();
}

const startConnection = async () => {
    try {
        disableControls(true);
        showConnectionStateMessage("Connecting...", "connecting");
        await connection.start();
        if (conversationId) {
           await connection.invoke("JoinTheGroupAsync", conversationId);
        } else {
            const url = new URL(client);
            await connection.invoke("CreateConversationAsync", {
                FirstMessage: messageInput.value,
                Host: url.hostname,
                Path: url.pathname,
                SubscriptionId: sub
            });
            clearInput();
        }
        hideConnectionStateMessage();
    } catch (err) {
        console.error(err);
        setTimeout(() => startConnection(), 2000);
    }
};


const sendMessage = async () => {
    try {
        await connection.invoke("SendMessageAsync", messageInput.value, conversationId);
        clearInput();
    } catch (err) {
        console.error(err);
    }
};

const handleFormSubmit = async (e) => {
    if (e.key && e.key !== "Enter") return;
    if (!messageInput.value) return;

    if (conversationId) {
        await sendMessage();
    } else {
        await startConnection();
    }
};

const showConnectionStateMessage = (text, connectionState) => {
    let messageTag = document.querySelector(".chat-connection-message");
    if (!messageTag) messageTag = document.createElement("div");
    messageTag.textContent = text;
    messageTag.classList.add("chat-connection-message");
    messageTag.classList.add(`chat-connection-message--${connectionState}`);
    chatContent.prepend(messageTag);
}

const hideConnectionStateMessage = () => {
    document.querySelector(".chat-connection-message").remove();
}

connection.onclose(() => {
    showConnectionStateMessage("Disconnected. To connect again, refresh the page. ", "disconnected");
    disableControls(true);
});

connection.onreconnecting(() => {
    showConnectionStateMessage("Reconnecting...","reconnecting");
    disableControls(true);
});

connection.onreconnected(async () => {
    await connection.invoke("JoinTheGroupAsync", conversationId);
    showConnectionStateMessage("Reconnected", "reconnected");
    disableControls(false);
    setTimeout( () =>{
        hideConnectionStateMessage();
    }, 1500);
});

connection.on("JoinedTheGroup", (joinToConversationViewModel) => {
  sessionStorage.setItem(CONNECTION_ID_STORAGE_NAME, joinToConversationViewModel.conversationId);
  conversationId = joinToConversationViewModel.conversationId;
  
  const fragment = document.createDocumentFragment();

    joinToConversationViewModel.messages.forEach((message) => {
       const messageTemplate = makeMessageTemplate(message);
       fragment.appendChild(messageTemplate);
    });

    messageList.appendChild(fragment);
    messageList.scrollTo(0, messageList.scrollHeight);
   disableControls(false);
});

connection.on("ReceiveMessageAsync", (message) => {
    appendMessage(message);
});

connection.on("OnCloseConversationAsync", () => {
    sessionStorage.removeItem(CONNECTION_ID_STORAGE_NAME);
    showConnectionStateMessage(`Conversation has been closed. Refresh 
    page and send a new message to start new conversation.`, "disconnected");
    });

if (conversationId) await startConnection();
sendButton.addEventListener("click", handleFormSubmit);
messageInput.addEventListener("keydown", handleFormSubmit);
