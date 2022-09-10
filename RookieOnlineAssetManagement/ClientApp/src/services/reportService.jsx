import axios from "axios";
import fileDownload from "js-file-download";
const API_URL = "/api";

const exportToExcel = () => {
  axios
    .get(`${API_URL}/reports/ExportToExcel`, {
      responseType: "blob",
    })
    .then((res) => {
      fileDownload(res.data, "Reports.xlsx");
    });
};

export default {
  exportToExcel,
};
