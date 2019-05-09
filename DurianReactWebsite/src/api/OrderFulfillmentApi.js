import axios from "axios";

const OrderFulfillmentApi = async orderId => {
  await axios.put(`https://cmpe-durian.azurewebsites.net/api/order/${orderId}`);
};

export default OrderFulfillmentApi;
