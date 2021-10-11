const PAGINATION_ACTIONS = {
    STEP: "step",
    SET:"set"
};

const CalcPageIndex = (currentIndex, action, value) => {
    switch (action) {
        case PAGINATION_ACTIONS.STEP:
            return currentIndex + value;
        case PAGINATION_ACTIONS.SET:
            return value;
        default:
            return currentIndex;
    }
};

const SetNumbersEventListeners = (
    callback,
    numberSelector = ".pagination-number",
) => {
    const NUMBER_TAGS = document.querySelectorAll(numberSelector);

    NUMBER_TAGS.forEach(t => {
        t.addEventListener("click", () => callback(PAGINATION_ACTIONS.SET, t.dataset.index));
    });

};

const SetPaginationEventListeners = (
    callback,
    prevSelector = "#pagination-prev",
    numberSelector =".pagination-number",
    nextSelector = "#pagination-next",
) => {

    const PREV_TAG = document.querySelector(prevSelector);
    const NEXT_TAG = document.querySelector(nextSelector);

    PREV_TAG.addEventListener("click", () => callback(PAGINATION_ACTIONS.STEP, -1));
    NEXT_TAG.addEventListener("click", () => callback(PAGINATION_ACTIONS.STEP, 1));
    SetNumbersEventListeners(callback, numberSelector);

};

const SurroundingIndexTemplate = (index, isActive) => {
    return `
            <li class="page-item ${isActive?'active':''}">
                <span
                     class="page-link pagination-number rounded-0"
                     data-index="${index}"
                   >
                ${index}
                </span>
            </li>`;
};

const DisplaySurroundingIndexes = (
    currentIndex,
    surroundingIndexes,
    paginationIndexesContainer = "#pagination-numbers-containers"
) => {

    const CONTAINER = document.querySelector(paginationIndexesContainer);
    const fragment = document.createDocumentFragment();

    surroundingIndexes.forEach(index => {
        const node = document.createElement("div");
        node.innerHTML = SurroundingIndexTemplate(index, currentIndex === index);
        fragment.appendChild(node);
    });

    CONTAINER.innerHTML = "";
    CONTAINER.appendChild(fragment);

};

const UpdateStepBtnState = (btnSelector, isActive) => {
    const BTN = document.querySelector(btnSelector);
    if (isActive) {
        BTN.classList.remove("disabled");
    } else {
        BTN.classList.add("disabled");
    }
};

const UpdateStepBtnsStates = (
    hasPreviousPage,
    hasNextPage,
    prevSelector = "#pagination-prev-wrapper",
    nextSelector = "#pagination-next-wrapper",
) => {
    UpdateStepBtnState(prevSelector, hasPreviousPage);
    UpdateStepBtnState(nextSelector, hasNextPage);
};

const UpdatePagination = (
    callback,hasPreviousPage, hasNextPage, currentIndex, surroundingIndexes,
    paginationIndexesContainer, prevSelector, nextSelector, numberSelector
) => {
    UpdateStepBtnsStates(hasPreviousPage, hasNextPage, prevSelector, nextSelector);
    DisplaySurroundingIndexes(currentIndex, surroundingIndexes, paginationIndexesContainer);
    SetNumbersEventListeners(callback, numberSelector);
};
