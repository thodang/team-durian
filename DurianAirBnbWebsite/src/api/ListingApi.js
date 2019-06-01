import axios from "axios";

const ListingApi = async (from, count, guests) => {
  const response = await axios.get(
    `https://cmpe-term-project.azurewebsites.net/api/listing/paged?from=${from}&count=${count}&guests=${guests}`
    //`http://localhost:55615/api/listing/paged?from=${from}&count=${count}&guests=${guests}`
  );
  const data = await response.data;
  return data;
};

export default ListingApi;
