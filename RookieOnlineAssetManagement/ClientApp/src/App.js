import axios from "axios";
import { useEffect, useState } from "react";
import { Route, Routes, Navigate } from "react-router-dom";

import "./App.css";
import loading from "./assets/loading.gif";
import FirstPassmodal from "./components/FirstPassmodal/FirstPassmodal";
import Navbar from "./components/Navbar/Navbar";
import Sidebar from "./components/Sidebar/Sidebar";
import CreateAsset from "./pages/Assets/CreateAsset/CreateAsset";
import UpdateAsset from "./pages/Assets/UpdateAsset/UpdateAsset";
import Home from "./pages/Home/Home";
import CreateUser from "./pages/Users/CreateUser";
import UserTable from "./pages/Users/ListUser";
import UpdateUser from "./pages/Users/UpdateUser";
import AssetTable from "./pages/Assets/ListAsset";
import RequestTable from "./pages/Request/ListRequest";

import CreateAssignment from "./pages/Assignments/CreateAssignment";
import UpdateAssignment from "./pages/Assignments/UpdateAssignment";
import AssignmentTable from "./pages/Assignments/ListAssignment/Index";
import ReportTable from "./pages/Report/ListReport";

export default function App() {
  const [id, setId] = useState(null);
  const [username, setUsername] = useState(null);
  const [role, setRole] = useState("");
  const [firstLogin, setFirstLogin] = useState(false);
  axios.interceptors.request.use((config) => {
    return config;
  });
  axios.interceptors.response.use(
    (response) => {
      return response;
    },
    (error) => {
      if (401 === error.response.status) {
        window.location.href =
          "/Identity/Account/Login?returnUrl=" + window.location.pathname;
      } else {
        return Promise.reject(error);
      }
    }
  );

  useEffect(() => {
    axios.get("/api/users/get-user").then((response) => {
      setId(response.data.id);
      setUsername(response.data.userName);
      setRole(response.data.role[0]);
      setFirstLogin(response.data.firstLogin);
    });
  }, []);
  return (
    <div>
      {role === "Admin" ? (
        <>
          <Navbar username={username} />
          <div className="body">
            <Sidebar role={role} />
            <Routes>
              <Route exact path="/" element={<Home userId={id} />} />
              <Route path="/users" element={<UserTable />} />
              <Route path="/users/add" element={<CreateUser />} />
              <Route path="/users/update/:id" element={<UpdateUser />} />
              <Route path="/assignments" element={<AssignmentTable />} />
              <Route path="/assignments/add" element={<CreateAssignment />} />
              <Route
                path="/assignments/update/:id"
                element={<UpdateAssignment />}
              />
              <Route path="/assets/add" element={<CreateAsset />} />
              <Route path="/assets/update/:id" element={<UpdateAsset />} />
              <Route path="/assets" element={<AssetTable />} />

              <Route path="/reports" element={<ReportTable />} />
              <Route path="/requests" element={<RequestTable />} />
            </Routes>
          </div>
        </>
      ) : role === "Staff" ? (
        <>
          <Navbar username={username} />
          <div className="body">
            <Sidebar role={role} />
            <Routes>
              <Route exact path="/" element={<Home userId={id} />} />
              <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
          </div>
        </>
      ) : (
        <>
          <div className="loading">
            <img src={loading} alt="Loading..." />
          </div>
        </>
      )}
      {firstLogin ? <FirstPassmodal id={id} /> : <div></div>}
    </div>
  );
}
