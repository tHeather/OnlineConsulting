const formTag = document.getElementById("form");
const AverageTimeConsultantJoiningCounter = document.getElementById("AverageTimeConsultantJoiningCounter");
const AverageConversationDuration = document.getElementById("AverageConversationDuration");
const InProgressConversationsNumber = document.getElementById("InProgressConversationsNumber");
const NewConversationsNumber = document.getElementById("NewConversationsNumber");
const loaders = document.querySelectorAll(".loader");
const counterNumbers = document.querySelectorAll(".counterNumber");
const plugin = {
    id: 'custom_canvas_background_color',
    beforeDraw: (chart) => {
        const ctx = chart.canvas.getContext('2d');
        ctx.save();
        ctx.globalCompositeOperation = 'destination-over';
        ctx.fillStyle = '#252525';
        ctx.fillRect(0, 0, chart.width, chart.height);
        ctx.restore();
    }
};
const chart = new Chart(
    document.getElementById('chart'),
    {
        type: 'line',
        plugins: [plugin],
        options: {
            plugins:{
                legend: {
                    labels: {
                        color: "#bebfc5"
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        borderColor: "#6c757d",
                        color: "#6c757d",
                    },
                    ticks: {
                        color:"#bebfc5"
                    }
                },
                y: {
                    grid: {
                        borderColor: "#6c757d",
                        color: "#6c757d"
                    },
                    ticks: {
                        color: "#bebfc5"
                    }
                },
            },
        }
    }
);
document.getElementById("StartDate").value = dayjs().add(-1,'day').format("YYYY-MM-DD");
document.getElementById("EndDate").value = dayjs().format("YYYY-MM-DD");


const changeLoadersState = (areLoadersVisible) => {
    const loadersDisplay = areLoadersVisible ? "inline-block" : "none";
    const countersDisplay = areLoadersVisible ? "none" :"inline";
    counterNumbers.forEach(counter => counter.style.display = countersDisplay);
    loaders.forEach(loader => loader.style.display = loadersDisplay);
}

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

const isDateValid = ({ StartDate, EndDate }) => {
    const isValid = new Date(StartDate).getTime() <= new Date(EndDate).getTime();
    if (!isValid) alert("End date must be greater than start date.");
    return isValid;
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

const updateChart = ({
    allConversations,
    servedConversations,
    notServedConversations
}) => {

    const allConversationsDataSet = makeDataset(allConversations, "#1d8cf8", "All");
    const servedConversationsDataSet = makeDataset(servedConversations, "#00f2c3", "Served");
    const notServedConversationsDataSet = makeDataset(notServedConversations, "#f53794", "Closed, not served");

    const dates = allConversations.map(c => dayjs.utc(c.date, "YYYY-MM-DD HH:mm").local().format("DD-MM-YYYY"));

     const data = {
        labels: dates,
         datasets: [allConversationsDataSet, servedConversationsDataSet, notServedConversationsDataSet]
    };

    chart.data = data;
    chart.update();
}


const formatCounterValues = ({ hours, minutes, seconds }) => {
    const hoursText = hours > 0 ? `${hours} H ` : "";
    const minutesText = minutes > 0 ? `${minutes} M ` : "";
    const secondsText = seconds > 0 ? `${seconds} S` : "";
    return`${hoursText}${minutesText}${secondsText}`;
}

const updateCounters = ({
    averageTimeConsultantJoining,
    averageConversationDuration,
    inProgressConversationsNumber,
    newConversationsNumber
}) => {
    AverageTimeConsultantJoiningCounter.innerText = formatCounterValues(averageTimeConsultantJoining);
    AverageConversationDuration.innerText = formatCounterValues(averageConversationDuration);
    InProgressConversationsNumber.innerText = inProgressConversationsNumber;
    NewConversationsNumber.innerText = newConversationsNumber;
}

const updateStatistics = async () => {

    const formValues = getFormValues();
    if (!isDateValid(formValues)) return;
    changeLoadersState(true);
    const statistics = await getStatistics(formValues);
    updateChart(statistics);
    updateCounters(statistics);
    changeLoadersState(false);
}

const submitForm = (e) => {
    e.preventDefault();
    updateStatistics();
}


updateStatistics();
formTag.addEventListener("submit", submitForm);

