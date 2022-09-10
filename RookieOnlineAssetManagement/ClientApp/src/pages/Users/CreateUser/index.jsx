import React, { useState } from "react";
import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as yup from "yup";

import userService from "../../../services/userService";
import Modal from "react-bootstrap/Modal";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

import moment from 'moment';
function Index(props) {
  const navigate = useNavigate();
  const [cancel, setCancel] = useState(false);

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
    if (value) {
      if (value.getDay() === 6 || value.getDay() === 0) {
        return true;
      }
      return false;
    } else {
      return;
    }
  };

  function getAgeJoin(dateOfBirth, dateJoin) {
    var today = new Date(dateJoin);
    var birthDate = new Date(dateOfBirth);
    var age = today.getFullYear() - birthDate.getFullYear();
    console.log(age);
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  }
  //set date future
  const today = moment();
  const disableFutureDt = current => {
    return current.isBefore(today)
  }

  const formik = useFormik({
    initialValues: {
      firstName: "",
      lastName: "",
      dateOfBirth: "",
      gender: "",
      joinedDate: "",
      type: "",
    },
    validationSchema: yup.object({
      firstName: yup
        .string()
        .required("First name is required")
        .min(2, "First name should be more than 2 characters")
        .max(50, "First name should be less than 50 characters")
        .matches(
          /^[a-zA-Z ]*$/,
          "First name should not contain number or special characters"
        ),
      lastName: yup
        .string()
        .required("Last name is required")
        .min(2, "Last name should be more than 2 characters")
        .max(50, "Last name should be less than 50 characters")
        .matches(
          /^[a-zA-Z ]*$/,
          "Last name should not contain number or special characters"
        ),
      dateOfBirth: yup
        .date()
        .required("Date Of Birth is required")
        .test(
          "dateOfBirth",
          "User is under 18. Please select a different date",
          (value) => {
            return getAge(value) >= 18;
          }
        ),
      gender: yup.string().required("Gender is required"),
      joinedDate: yup
        .date()
        .required("Joined Date is required")
        .min(
          yup.ref("dateOfBirth"),
          "Joined date is not later than Date of Birth. Please select a different date"
        )
        .test(
          "joinedDate",
          "Joined date is Saturday or Sunday. Please select a different date",
          (value) => {
            return getSaturdayOrSunday(value) === false;
          }
        )
        .test({
          name: "test",
          exclusive: false,
          params: {},
          message:
            "User must be 18 years old to join. Please select a different date",
          test: function (value) {
            return getAgeJoin(this.parent.dateOfBirth, value) >= 18;
          },
        }),
      type: yup.string().required("Type is required"),
    }),
    onSubmit: (data) => {
      //const user = new FormData();

      userService
        .create(data)
        .then((response) => {
          localStorage.setItem("identifier", "userCreated");
          navigate("/users", { state: { data } });
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
    <div className="user-creation">
      <h2 className="text-danger fw-bold mb-4" style={{ fontSize: 24 }}>
        Create New User
      </h2>
      <form
        className="form-create needs-validation"
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
              type="text"
              id="firstName"
              className={`form-control ${
                formik.errors.firstName && formik.touched.firstName === true
                  ? "is-invalid"
                  : ""
              }`}
              name="firstName"
              value={formik.values.firstName}
              onChange={formik.handleChange}
            />
            {formik.errors.firstName && formik.touched.firstName && (
              <p className="text-danger mb-0">{formik.errors.firstName}</p>
            )}
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
              type="text"
              id="lastName"
              className={`form-control ${
                formik.errors.lastName && formik.touched.lastName === true
                  ? "is-invalid"
                  : ""
              }`}
              name="lastName"
              value={formik.values.lastName}
              onChange={formik.handleChange}
            />
            {formik.errors.lastName && formik.touched.lastName && (
              <p className="text-danger mb-0">{formik.errors.lastName}</p>
            )}
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
              className={`form-control ${
                formik.errors.dateOfBirth && formik.touched.dateOfBirth === true
                  ? "is-invalid"
                  : ""
              }`}
              name="dateOfBirth"
              value={formik.values.dateOfBirth}
              onChange={formik.handleChange}
            />
            {formik.errors.dateOfBirth && formik.touched.dateOfBirth && (
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
                className={`form-check-input ${
                  formik.errors.gender && formik.touched.gender === true
                    ? "is-invalid"
                    : ""
                }`}
                name="gender"
                value="2"
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
                className={`form-check-input ${
                  formik.errors.gender && formik.touched.gender === true
                    ? "is-invalid"
                    : ""
                }`}
                name="gender"
                value="1"
                // checked={formik.values?.gender}
                onChange={formik.handleChange}
              />
            </div>
            {formik.errors.gender && formik.touched.gender && (
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
              className={`form-control ${
                formik.errors.joinedDate && formik.touched.joinedDate === true
                  ? "is-invalid"
                  : ""
              }`}
              name="joinedDate"
              value={formik.values.joinedDate}
              max={new Date().toISOString().slice(0,10)}
              onChange={formik.handleChange}
            />
            {formik.errors.joinedDate && formik.touched.joinedDate && (
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
              className={`form-select ${
                formik.errors.type && formik.touched.type === true
                  ? "is-invalid"
                  : ""
              }`}
              id="type"
              name="type"
              value={formik.values.type}
              onChange={formik.handleChange}
            >
              <option value="">Please select type</option>
              <option value="Admin">Admin</option>
              <option value="Staff">Staff</option>
            </select>
            {formik.errors.type && formik.touched.type && (
              <p className="text-danger mb-0">{formik.errors.type}</p>
            )}
          </div>
        </div>
        <div className="d-flex justify-content-end">
          <button
            className={`btn save-button ${
              formik.values.firstName !== "" &&
              formik.values.lastName !== "" &&
              formik.values.dateOfBirth !== "" &&
              formik.values.gender !== "" &&
              formik.values.joinedDate !== "" &&
              formik.values.type !== ""
                ? ""
                : "disabled"
            }`}
            type="submit"
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
          <p> Do you want to cancel create user?</p>
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
