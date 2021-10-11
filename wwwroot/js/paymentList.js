const PAYMENT_LIST = document.getElementById("paymentList");
const FORM = document.getElementById("FiltersForm");
const LIST_LOADER = document.getElementById("ListLoader");
const API_URL = "/api/payment/list";
let currentIndex = 1;

const PAYMENT_TEMPLATE = ({
    id, createDate, price, status, dotPayOperationNumber, subscriptionType, email
}) => {

    const date = dayjs
        .utc(createDate, 'YYYY-MM-DD HH:mm')
        .local().format('DD-MM-YYYY HH:mm');

    return `                   
            <td class="text-light" >${id}</td>
            <td class="text-light" >${date}</td>
            <td class="text-light" >${price}</td>
            <td class="text-light" >${status}</td>
            <td class="text-light" >${dotPayOperationNumber}</td>
            <td class="text-light" >${subscriptionType}</td>
            <td class="text-light" >${email}</td>
          `;
};

const DisplayPayments = (payments) => {

    PAYMENT_LIST.innerHTML = "";

    const fragment = document.createDocumentFragment();

    payments.forEach(payment => {
        const node = document.createElement("tr");
        node.innerHTML = PAYMENT_TEMPLATE(payment);
        fragment.appendChild(node);
    });

    PAYMENT_LIST.appendChild(fragment);
};

const GetPayments = async (action, value) => {

    $("#collapseFilters").collapse('hide');

    SetLoaderState(true);

    const formValues = GetFormWithDateRangeValues("#FiltersForm", "StartDate", "EndDate");

    formValues.PageIndex = CalcPageIndex(currentIndex, action, value);

    const adminPaymentList = await JsonFetchWithBasicHandler(
        `${API_URL}?${MakeQueryString(formValues)}`,
        "GET");

    const { list, pageIndex, hasPreviousPage, hasNextPage, surroundingIndexes } = adminPaymentList;

    currentIndex = pageIndex;

    DisplayPayments(list);

    UpdatePagination(
        GetPayments, hasPreviousPage, hasNextPage,pageIndex, surroundingIndexes
    );

    SetLoaderState(false);

};

const SubmitForm =  async (e) => {
   if(e) e.preventDefault();
   await GetPayments();
};

const SetLoaderState = (isVisible) => {
    LIST_LOADER.style.display = isVisible ? "block" : "none";
    PAYMENT_LIST.style.display = isVisible ? "none" : "table-row-group";
};

SetLoaderState(true);
document.getElementById("StartDate").value = dayjs().add(-1, 'day').format("YYYY-MM-DD");
document.getElementById("EndDate").value = dayjs().format("YYYY-MM-DD");
FORM.addEventListener("submit", SubmitForm);
SetPaginationEventListeners(GetPayments);
SubmitForm();

