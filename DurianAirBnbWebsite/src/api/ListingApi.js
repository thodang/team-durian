import axios from "axios";

const ListingApi = async (from, count) => {
  const response = await axios.get(
    `http://localhost:55615/api/listing/paged?from=${from}&count=${count}`
  );
  const data = await response.data;
  return data;
};

export default ListingApi;
