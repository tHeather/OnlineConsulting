const JsonFetch = async (url, method, data) => {
    let requestInfo = {
        method: method,
        headers: {
            "Content-Type": "application/json",
        },
        credentials: "same-origin",
        redirect: "follow",
    };

    if (data) requestInfo.body = JSON.stringify(data);

    const response = await fetch(url, requestInfo);

    return response;
};

const MakeQueryString = (paramsObj) => {
    let paramsString = "";
    for (const property in paramsObj) {
        if (paramsObj[property] !== "") {
            paramsString += `&${property}=${paramsObj[property]}`;
        }
    }
    return paramsString.substring(1);
};

const GetFormWithDateRangeValues = (formSelector, startDateName, endDateName ) => {

    const values = {};
    const inputs = document.querySelector(formSelector).elements;

    for (let i = 0; i < inputs.length; i++) {
        switch (inputs[i].name) {
            case startDateName:
                values[inputs[i].name] = dayjs(inputs[i].value).utc().format();
                break;
            case endDateName:
                values[inputs[i].name] = dayjs(inputs[i].value).add(1, 'day').utc().format();
                break;
            default:
                values[inputs[i].name] = inputs[i].value;
        }
    }

    return values;
}

const JsonFetchWithBasicHandler = async (url, method, data) => {
    try {
        const response = await JsonFetch(url, method, data);
        switch (response.status) {
            case 200:
                return await response.json();
            case 500:
                window.location.href = '/server-error';
                break;
            default:
        }
    } catch (error) {
        console.error(error);
    }
}