const formTag = document.getElementById("form");
const chart = new Chart(
    document.getElementById('chart'),
    {
        type: 'line',
        data: {},
        options: {}
    }
);
document.getElementById("StartDate").value = dayjs().add(-1,'day').format("YYYY-MM-DD");
document.getElementById("EndDate").value = dayjs().format("YYYY-MM-DD");

const makeQueryString = (paramsObj) => {
    let paramsString = "";
    for (const property in paramsObj) {
        if (paramsObj[property] !== "") {
            paramsString += `&${property}=${paramsObj[property]}`;
        }
    }
    return paramsString.substring(1);
};

const fetchData = async (params) => {

return await fetch(
    `/api/statistics/get-statistics?${makeQueryString(params)}`,
    {
      method: "GET",
      headers: {
                 "Content-Type": "application/json",
               },
      credentials: "same-origin",
      redirect: "follow",
    });
};

const getStatistics = async (params) => {

    try {
        const response = await fetchData(params);
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


const getFormValues = () => {

    const values = {};
    const inputs = document.getElementById("form").elements;

    for (let i = 0; i < inputs.length; i++) {
        switch (inputs[i].name) {
            case "StartDate":
                values[inputs[i].name] = dayjs(inputs[i].value).utc().format();
                break;
            case "EndDate":
                values[inputs[i].name] = dayjs(inputs[i].value).add(1, 'day').utc().format();
                break;
            default:
                values[inputs[i].name] = inputs[i].value;
        }
    }

    return values;
}


const makeDataset = (statisticList, color, label) => {

    const data = statisticList.map(s => s.count);

    return {
        label,
        backgroundColor: color,
        borderColor: color,
        data
    };
}

const updateDatasets = ({
    allConversations,
    servedConversations,
    notServedConversations
}) => {

    const allConversationsDataSet = makeDataset(allConversations, "#4dc9f6", "All");
    const servedConversationsDataSet = makeDataset(servedConversations, "#f67019", "Served");
    const notServedConversationsDataSet = makeDataset(notServedConversations, "#f53794", "Closed, not served");

    const dates = allConversations.map(c => dayjs.utc(c.date, "YYYY-MM-DD HH:mm").local().format("DD-MM-YYYY"));

     const data = {
        labels: dates,
         datasets: [allConversationsDataSet, servedConversationsDataSet, notServedConversationsDataSet]
    };

    chart.data = data;
    chart.update();
}

const updateChart = async () => {
    const formValues = getFormValues();
    const statistics = await getStatistics(formValues);
    updateDatasets(statistics);
}

const submitForm = (e) => {
    e.preventDefault();
    updateChart();
}


updateChart();
formTag.addEventListener("submit", submitForm);

