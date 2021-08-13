const sendButton = document.getElementById("chat-send-button");
const messageInput = document.getElementById("chat-message-input");
const messageList = document.getElementById("chat-messages-list");
const chatIconButton = document.getElementById("chat-icon");

/* WIDGET */
const urlSearchParams = new URLSearchParams(window.location.search);
const { client } = Object.fromEntries(urlSearchParams.entries());

const handleClick = () => {
  document
    .getElementById("chat-content")
    .classList.toggle("chat-content--open");
  window.parent.postMessage("TOGGLE", client);
};

chatIconButton.addEventListener("click", handleClick);

/* SIGNALR */
const connection = new signalR.HubConnectionBuilder()
  .withUrl("/chatHub")
  .withAutomaticReconnect()
  .build();

const startConnection = () => {
  connection
    .start()
    .then(function () {
      sendButton.disabled = false;
      console.log(`connected:${connection.connectionId}`);
    })
    .catch(function (err) {
      return console.error(err.toString());
    });
};

const sendMessage = () => {
  connection
    .invoke("SendMessageAsync", messageInput.value, null)
    .then(() => {
      messageInput.value = "";
      textAreaAdjust();
    })
    .catch((err) => {
      return console.error(err.toString());
    });
};

startConnection();

connection.on(
  "ReceiveMessageAsync",
  ({ createDate, content, isFromClient }) => {
    const li = document.createElement("li");
    const messageTemplate = `
        <div>${content}</div>
        <div class="chat-messages-list__date" data-utc-date="${createDate}"></div>
        `;

    li.innerHTML = messageTemplate;
    li.classList.add("chat-messages-list__message");
    if (isFromClient) li.classList.add("chat-messages-list__message--client");

    if (messageList.lastChild) {
      messageList.lastChild.after(li);
    } else {
      messageList.appendChild(li);
    }

    convertUtcToLocalDate();
  }
);

connection.onclose(() => {
  console.log("disconnected");
});

connection.onreconnecting((error) => {
  console.log(`reconnecting:${connection.connectionId}`);
});

connection.onreconnected((error) => {
  console.log(`onreconnected :${connection.connectionId}`);
});

const handleFormSumit = (e) => {
  if (connection.connectionState !== "Connected") return;
  if (e.key && e.key !== "Enter") return;
  if (!messageInput.value) return;
  sendMessage();
};

sendButton.addEventListener("click", handleFormSumit);
messageInput.addEventListener("keydown", handleFormSumit);

/* FORM */
const textAreaAdjust = () => {
  messageInput.style.height = "1px";
  let scroll =
    messageInput.scrollHeight > 100 ? 100 : messageInput.scrollHeight;
  messageInput.style.height = `${scroll}px`;
  messageInput.style.overflowY = scroll === 100 ? "scroll" : "hidden";
};

messageInput.addEventListener("keyup", textAreaAdjust);

const convertUtcToLocalDate = () => {
  const dateTags = document.querySelectorAll("[data-utc-date]");
  dateTags.forEach((dateTag) => {
    dateTag.innerText = moment
      .utc(dateTag.dataset.utcDate, "DD-MM-YYYY HH:mm")
      .local()
      .format("HH:mm");
  });
};
