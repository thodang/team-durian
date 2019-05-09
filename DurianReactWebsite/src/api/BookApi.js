import axios from "axios";

const BookApi = async () => {
  const response = await axios.get(
    "https://cmpe-durian.azurewebsites.net/api/book/available"
  );
  const data = await response.data;
  return data;
};

export default BookApi;
