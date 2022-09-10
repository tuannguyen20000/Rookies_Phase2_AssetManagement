import axios from "axios";

const API_URL = "/api";

const completeRequest = (id) => {
  return axios.put(API_URL + `/requests/complete-request/${id}`);
};

const cancelRequest = (id) => {
  return axios.put(API_URL + `/requests/cancel-request/${id}`);
};

export default {
  completeRequest,
  cancelRequest,
};
