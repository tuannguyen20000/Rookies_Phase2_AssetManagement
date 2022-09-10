import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useFormik } from "formik";

import userService from "../../../services/userService";
import Modal from "react-bootstrap/Modal";
import * as yup from "yup";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const formatDate = (date) => {
  var d = new Date(date),
    month = "" + (d.getMonth() + 1),
    day = "" + d.getDate(),
    year = d.getFullYear();

  if (month.length < 2) month = "0" + month;
  if (day.length < 2) day = "0" + day;

  return [year, month, day].join("-");
};

function getAge(dateString) {
  var today = new Date();
  var birthDate = new Date(dateString);
  var age = today.getFullYear() - birthDate.getFullYear();
  var m = today.getMonth() - birthDate.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
    age--;
  }
  return age;
}

const getSaturdayOrSunday = (value) => {
  if (value.getDay() === 6 || value.getDay() === 0) {
    return true;
  }
  return false;
};
function Index() {
  var { id } = useParams();
  const userDefualt = {
    id: null,
    firstName: "",
    lastName: "",
    dateOfBirth: "",
    gender: "",
    joinedDate: "",
    type: "",
  };
  let [user, setUser] = useState(userDefualt);
  const getUser = (id) => {
    userService
      .getDetail(id)
      .then((res) => {
        formik.setFieldValue("firstName", res.data.firstName);
        formik.setFieldValue("lastName", res.data.lastName);
        formik.setFieldValue("dateOfBirth", formatDate(res.data.dateOfBirth));
        formik.setFieldValue("gender", res.data.gender);
        formik.setFieldValue("joinedDate", formatDate(res.data.joinedDate));
        formik.setFieldValue("type", res.data.type);
        //setUser(res.data)
      })
      .catch((e) => console.log(e));
  };
  useEffect(() => {
    getUser(id);
  }, [id]);
  const [cancel, setCancel] = useState(false);
  const navigate = useNavigate();
  const formik = useFormik({
    initialValues: {
      firstName: null,
      lastName: null,
      dateOfBirth: null,
      gender: null,
      joinedDate: null,
      type: null,
    },
    validationSchema: yup.object({
      firstName: yup.string().required("First name is required"),
      lastName: yup.string().required("Last name is required"),
      dateOfBirth: yup
        .date()
        .required("dateOfBirth is required")
        .test(
          "dateOfBirth",
          "User is under 18. Please select a different date",
          (value) => {
            if (!value) return false;
            return getAge(value) >= 18;
          }
        ),
      gender: yup.string().required("Gender is required"),
      joinedDate: yup
        .date()
        .required("joinedDate is required")
        .min(
          yup.ref("dateOfBirth"),
          "Joined date is not later than Date of Birth. Please select a different date"
        )
        .test(
          "joinedDate",
          "Joined date is Saturday or Sunday. Please select a different date",
          (value) => {
            if (!value) return false;
            return getSaturdayOrSunday(value) === false;
          }
        ),
      type: yup.string().required("Type is required"),
    }),
    onSubmit: (data) => {
      let userFromData = new FormData();
      userFromData.append("DateOfBirth", data.dateOfBirth);
      userFromData.append("Gender", data.gender);
      userFromData.append("JoinedDate", data.joinedDate);
      userFromData.append("Type", data.type);
      userFromData.append("Id", id);
      userService
        .update(user.id, userFromData)
        .then((response) => {
          localStorage.setItem("identifier", "userCreated");
          navigate("/users");
        })
        .catch((e) => {
          console.log(e);
        });
    },
  });

  const handleCancel = () => {
    setCancel(true);
  };

  const handleReturn = () => {
    setCancel(false);
  };

  return (
    <div className="update-user-div col-md-9" style={{ textAlign: "center" }}>
      <h2 className="text-danger fw-bold mb-4" style={{ fontSize: 24 }}>
        Edit User
      </h2>
      <form
        className="form-update needs-validation"
        onSubmit={formik.handleSubmit}
      >
        <div className="row mb-3">
          <div className="col-1">
            <label className="col-form-label" htmlFor="firstName">
              First Name
            </label>
          </div>
          <div className="col-11">
            <input
              disabled
              type="text"
              id="firstName"
              className="form-control"
              name="firstName"
              value={formik.values.firstName}
              readOnly
            />
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="lastName" className="col-form-label">
              Last Name
            </label>
          </div>
          <div className="col-11">
            <input
              disabled
              type="text"
              id="lastName"
              className="form-control"
              name="lastName"
              value={formik.values.lastName}
              readOnly
            />
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1" style={{ paddingRight: 0 }}>
            <label htmlFor="dob" className="col-form-label">
              Date of Birth
            </label>
          </div>
          <div className="col-11">
            <input
              type="date"
              id="dob"
              className="form-control"
              name="dateOfBirth"
              value={formik.values.dateOfBirth}
              onChange={formik.handleChange}
            />
            {formik.errors.dateOfBirth && (
              <p className="text-danger mb-0">{formik.errors.dateOfBirth}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="gender" className="col-form-label">
              Gender
            </label>
          </div>
          <div className="gender-checkbox-container col-11">
            <div className="form-check form-check-inline">
              <label className="form-check-label" htmlFor="Female">
                Female
              </label>
              <input
                type="radio"
                id="Female"
                className="form-check-input"
                name="gender"
                value="Female"
                checked={formik.values.gender === "Female" ? true : false}
                onChange={formik.handleChange}
              />
            </div>
            <div className="form-check form-check-inline">
              <label className="form-check-label" htmlFor="Male">
                Male
              </label>
              <input
                type="radio"
                id="Male"
                className="form-check-input"
                name="gender"
                value="Male"
                checked={formik.values.gender === "Male" ? true : false}
                onChange={formik.handleChange}
              />
            </div>
            {formik.errors.gender && (
              <p className="text-danger mb-0">{formik.errors.gender}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="Joined" className="col-form-label">
              Joined Date
            </label>
          </div>
          <div className="col-11">
            <input
              type="date"
              id="Joined"
              className="form-control"
              name="joinedDate"
              value={formik.values.joinedDate}
              onChange={formik.handleChange}
            />
            {formik.errors.joinedDate && (
              <p className="text-danger mb-0">{formik.errors.joinedDate}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="type" className="col-form-label">
              Type
            </label>
          </div>
          <div className="col-11">
            <select
              className="form-select"
              id="type"
              name="type"
              value={formik.values.type}
              onChange={formik.handleChange}
            >
              <option value="">Please select type</option>
              <option value="Admin">Admin</option>
              <option value="Staff">Staff</option>
            </select>
            {formik.errors.type && (
              <p className="text-danger mb-0">{formik.errors.type}</p>
            )}
          </div>
        </div>
        <div className="d-flex justify-content-end">
          <button
            className="btn save-button"
            type="submit"
            onClick={formik.onSubmit}
          >
            Save
          </button>
          <button
            className="btn cancel-button"
            onClick={(e) => {
              e.preventDefault();
              handleCancel();
            }}
          >
            Cancel
          </button>
        </div>
      </form>
      <Modal
        show={cancel}
        onHide={handleReturn}
        backdrop="static"
        keyboard={false}
      >
        <Modal.Header>
          <Modal.Title>
            <p>Are you sure?</p>
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p> Do you want to cancel edit user?</p>
          <div className="modal-confirm-button-container">
            <button
              className="btn confirm-accept-button"
              style={{ width: "13%" }}
              onClick={() => navigate(`/users`)}
            >
              Yes
            </button>
            <button
              className="btn confirm-cancel-button"
              style={{ width: "13%" }}
              onClick={handleReturn}
            >
              No
            </button>
          </div>
        </Modal.Body>
      </Modal>
    </div>
  );
}

export default Index;
