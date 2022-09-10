import React, { useState, useEffect } from "react";
import { useFormik } from "formik";
import { useNavigate, useParams } from "react-router-dom";
import * as yup from "yup";
import { Input } from "antd";
import ModalUser from "../CreateAssignment/User/ModalUser";
import ModalAsset from "../CreateAssignment/Asset/ModalAsset";

import moment from "moment";
import axiosInstance from "../../../axios";
import Modal from "react-bootstrap/Modal";
import assignmentService from "../../../services/assignmentService";

import "bootstrap/dist/css/bootstrap.min.css";
import "../CreateAssignment/index.css";

function UpdateAssignment() {
  const navigate = useNavigate();
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  const { Search } = Input;
  const [isModalUserVisible, setIsModalUserVisible] = useState(false);
  const [isModalAssetVisible, setisModalAssetVisible] = useState(false);
  const [cancel, setCancel] = useState(false);
  const [selectedUser, setSelectedUser] = useState("");
  const [userId, setUserId] = useState();

  const [selectedAsset, setSelectedAsset] = useState("");
  const [assetId, setAssetId] = useState();

  const { id } = useParams();
  const getAssignment = (id) => {
    assignmentService
      .getDetailAssignment(id)
      .then((response) => {
        setSelectedUser(response.data.assignedTo);
        setSelectedAsset(response.data.assetName);
        setUserId(response.data.userId);
        setAssetId(response.data.assetId);
        formik.setFieldValue(
          "assignedDate",
          moment(response.data.assignedDate).format("yyyy-MM-DD")
        );
        formik.setFieldValue("note", response.data.note);
      })
      .catch((e) => console.log(e));
  };
  useEffect(() => {
    getAssignment(id);
  }, [id]);

  const handleSelectedUser = (infoUser) => {
    const { fullName } = infoUser;
    const { id } = infoUser;
    setSelectedUser(fullName);
    setUserId(id);
  };

  const handleSelectedAsset = (inforAsset) => {
    const { assetName } = inforAsset;
    const { id } = inforAsset;
    setSelectedAsset(assetName);
    setAssetId(id);
  };

  const showModalUser = () => {
    setIsModalUserVisible(true);
  };

  const showModalAsset = () => {
    setisModalAssetVisible(true);
  };

  const handleCancel = () => {
    setisModalAssetVisible(false);
    setIsModalUserVisible(false);
  };

  const onSearchUser = () => {
    showModalUser();
  };

  const onSearchAsset = () => {
    showModalAsset();
  };

  const formik = useFormik({
    initialValues: {
      assignedDate: "",
      note: "",
      userId: "",
      assetId: "",
    },
    validationSchema: yup.object({
      assignedDate: yup
        .date()
        .required("Assigned Date is required")
        .min(today, "Assigned Date is only current or future date"),
    }),
    onSubmit: (data) => {
      data.userId = userId;
      data.assetId = assetId;
      assignmentService
        .updateAssignment(id, data)
        .then((response) => {
          if (response != null) {
            localStorage.setItem("AssignmentIdentifier", "assignmentCreated");
            localStorage.setItem("AssignmentIdUpdated", response.data.id);
            navigate("/assignments");
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
  });

  const handleCancelUpdate = () => {
    setCancel(true);
  };

  const handleReturn = () => {
    setCancel(false);
  };

  return (
    <div className="assignment-creation">
      <h2 className="text-danger fw-bold mb-4" style={{ fontSize: 24 }}>
        Update Assignment
      </h2>
      <form
        className="form-create needs-validation"
        onSubmit={formik.handleSubmit}
      >
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="userId" className="col-form-label">
              User
            </label>
          </div>
          <div className="col-11">
            <Search onSearch={onSearchUser} value={selectedUser} readOnly />
            {formik.errors.userId && formik.touched.userId && (
              <p className="text-danger mb-0">{formik.errors.userId}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="assetId" className="col-form-label">
              Asset
            </label>
          </div>
          <div className="col-11">
            <Search onSearch={onSearchAsset} value={selectedAsset} readOnly />
            {formik.errors.assetId && formik.touched.assetId && (
              <p className="text-danger mb-0">{formik.errors.assetId}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="assignedDate" className="col-form-label">
              Assigned Date
            </label>
          </div>
          <div className="col-11">
            <input
              type="date"
              id="assignedDate"
              className={`form-control ${
                formik.errors.assignedDate &&
                formik.touched.assignedDate === true
                  ? "is-invalid"
                  : ""
              }`}
              min={moment(today).format("yyyy-MM-DD")}
              name="assignedDate"
              value={formik.values.assignedDate}
              onChange={formik.handleChange}
            />
            {formik.errors.assignedDate && formik.touched.assignedDate && (
              <p className="text-danger mb-0">{formik.errors.assignedDate}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-1">
            <label htmlFor="note" className="col-form-label">
              Note
            </label>
          </div>
          <div className="col-11">
            <textarea
              type="text"
              id="note"
              className={`form-control`}
              name="note"
              value={formik.values.note}
              onChange={formik.handleChange}
            />
            {formik.errors.note && formik.touched.note && (
              <p className="text-danger mb-0">{formik.errors.note}</p>
            )}
          </div>
        </div>
        <div className="d-flex justify-content-end">
          <button
            className={`btn save-button ${
              selectedUser !== "" &&
              selectedAsset !== "" &&
              formik.values.assignedDate !== ""
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
              handleCancelUpdate();
            }}
          >
            Cancel
          </button>
        </div>
      </form>
      <ModalUser
        selectedUser={handleSelectedUser}
        defaultUser={userId}
        visible={isModalUserVisible}
        handleCancel={handleCancel}
      />
      <ModalAsset
        visible={isModalAssetVisible}
        handleCancel={handleCancel}
        selectedAsset={handleSelectedAsset}
        defaultAsset={assetId}
      />
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
          <p> Do you want to cancel edit assignment?</p>
          <div className="modal-confirm-button-container">
            <button
              className="btn confirm-accept-button"
              style={{ width: "13%" }}
              onClick={() => navigate(`/assignments`)}
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

export default UpdateAssignment;
