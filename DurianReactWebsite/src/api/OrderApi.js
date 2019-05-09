import axios from "axios";

const OrderApi = async () => {
  const response = await axios.get(
    "https://cmpe-durian.azurewebsites.net/api/order/fulfillment"
  );
  const data = await response.data;
  return data;
};

export default OrderApi;
