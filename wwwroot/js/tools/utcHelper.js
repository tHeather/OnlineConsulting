const SetUtcInputs = (
    startDateSelector = '[name="StartDate"]',
    endDateSelector = '[name="EndDate"]',
    startDateUtcSelector = '[name="StartDateUtc"]',
    endDateUtcSelector = '[name="EndDateUtc"]',
    ) => {

    const startDateTag = document.querySelector(startDateSelector);
    const endDateTag = document.querySelector(endDateSelector);
    const startDateUtcTag = document.querySelector(startDateUtcSelector);
    const endDateUtcTag = document.querySelector(endDateUtcSelector);

    if (startDateTag.value) startDateUtcTag.value = dayjs(startDateTag.value).utc().format();
    if (endDateTag.value) endDateUtcTag.value = dayjs(endDateTag.value).add(1, 'day').utc().format();

};

const InitHiddenUtcInputs = (formSelector) => {
    const formTag = document.querySelector(formSelector);
    formTag.addEventListener("submit", () => SetUtcInputs());
};
