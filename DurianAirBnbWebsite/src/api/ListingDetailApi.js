import axios from "axios";

const ListingDetailApi = async id => {
  const response = await axios.get(
    `https://cmpe-term-project.azurewebsites.net/api/listing/${id}`
    //`http://localhost:55615/api/listing/${id}`
  );
  const data = await response.data;
  return data;
};

export default ListingDetailApi;
