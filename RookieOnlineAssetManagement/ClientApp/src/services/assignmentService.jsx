import axios from "axios";

const API_URL = "/api";

const createAssignment = (data) => {
  return axios.post(API_URL + "/assignment/create-assignment", data);
};
const updateAssignment = (id, data) => {
  return axios.put(API_URL + `/assignment/UpdateAssignment/${id}`, data);
};
const deleteAssignment = (id) => {
  return axios.delete(API_URL + `/Assignment/DeleteAssignment/${id}`);
};
const getDetailAssignment = (id) => {
  return axios.get(API_URL + `/assignment/DetailsAssignment/${id}`);
};
const getDetail = (id, sortOrder, sortField) => {
  return axios.get(
    API_URL + `/assignment/getuser-assignment/${id}/${sortOrder}/${sortField}`
  );
};
const acceptAssignment = (id) => {
  return axios.put(API_URL + `/assignment/accept/${id}`);
};
const declineAssignment = (id) => {
  return axios.delete(API_URL + `/assignment/DeleteAssignment/${id}`);
};
const requestCreate = (id) => {
  return axios.put(API_URL + `/requests/create-request/${id}`);
};

export default {
  createAssignment,
  updateAssignment,
  deleteAssignment,
  getDetailAssignment,
  getDetail,
  acceptAssignment,
  declineAssignment,
  requestCreate,
};
