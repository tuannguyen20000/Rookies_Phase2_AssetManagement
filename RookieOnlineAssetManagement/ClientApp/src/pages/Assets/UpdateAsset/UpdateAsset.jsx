import React, { useState, useEffect } from "react";
import { useFormik } from "formik";
import { useNavigate, useParams } from "react-router-dom";
import * as yup from "yup";

import assetService from "../../../services/assetService";
import Modal from "react-bootstrap/Modal";

import "bootstrap/dist/css/bootstrap.min.css";
import "./UpdateAsset.css";

const formatDate = (date) => {
  var d = new Date(date),
    month = "" + (d.getMonth() + 1),
    day = "" + d.getDate(),
    year = d.getFullYear();

  if (month.length < 2) month = "0" + month;
  if (day.length < 2) day = "0" + day;

  return [year, month, day].join("-");
};

const UpdateAsset = () => {
  const [cancel, setCancel] = useState(false);
  var { id } = useParams();
  const navigate = useNavigate();

  const getAsset = () => {
    assetService
      .getAsset(id)
      .then((res) => {
        formik.setFieldValue("assetName", res.data.assetName);
        formik.setFieldValue("category", res.data.categoryName);
        formik.setFieldValue("specification", res.data.specification);
        formik.setFieldValue(
          "installedDate",
          formatDate(res.data.installedDate)
        );
        formik.setFieldValue("state", res.data.state);
      })
      .catch((e) => console.log(e));
  };

  useEffect(() => {
    getAsset(id);
  }, []);

  const formik = useFormik({
    initialValues: {
      assetName: "",
      category: "",
      specification: "",
      installedDate: "",
      state: "",
    },
    validationSchema: yup.object({
      assetName: yup.string().required("Asset name is required"),
      specification: yup.string().required("Asset specification is required"),
      installedDate: yup.date().required("Installed date is required"),
      state: yup.string().required("State is required"),
    }),
    onSubmit: (data) => {
      assetService
        .updateAsset(id, data)
        .then((response) => {
          localStorage.setItem("assetIdentifier", "assetModify");
          localStorage.setItem("AssetIdUpdated", id);
          navigate("/assets");
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
    <div className="asset-container">
      <h2 className="text-danger fw-bold mb-4" style={{ fontSize: 24 }}>
        Edit Asset
      </h2>
      <form
        className="form-update needs-validation"
        onSubmit={formik.handleSubmit}
      >
        {/* name */}
        <div className="row mb-3">
          <div className="col-2">
            <label className="col-form-label" htmlFor="assetName">
              Name
            </label>
          </div>
          <div className="col-10">
            <input
              type="text"
              id="assetName"
              className={`form-control ${
                formik.errors.assetName && formik.touched.assetName === true
                  ? "is-invalid"
                  : ""
              }`}
              name="assetName"
              value={formik.values.assetName}
              onChange={formik.handleChange}
            />
            {formik.errors.assetName && formik.touched.assetName && (
              <p className="text-danger mb-0">{formik.errors.assetName}</p>
            )}
          </div>
        </div>
        {/* Category */}
        <div className="row mb-3">
          <div className="col-2">
            <label htmlFor="category" className="col-form-label">
              Category
            </label>
          </div>
          <div className="col-10">
            <input
              disabled
              type="text"
              id="category"
              className="form-control"
              name="category"
              value={formik.values.category}
              readOnly
            />
            <div className="h-100"></div>
          </div>
        </div>
        {/* Specification */}
        <div className="row mb-3">
          <div className="col-2">
            <label htmlFor="specification" className="col-form-label">
              Specification
            </label>
          </div>
          <div className="col-10">
            <textarea
              type="text"
              id="specification"
              className={`form-control ${
                formik.errors.specification &&
                formik.touched.specification === true
                  ? "is-invalid"
                  : ""
              }`}
              name="specification"
              value={formik.values.specification}
              onChange={formik.handleChange}
            />
            {formik.errors.specification && formik.touched.specification && (
              <p className="text-danger mb-0">{formik.errors.specification}</p>
            )}
          </div>
        </div>
        {/* installedDate */}
        <div className="row mb-3">
          <div className="col-2" style={{ paddingRight: 0 }}>
            <label htmlFor="installedDate" className="col-form-label">
              Installed Date
            </label>
          </div>
          <div className="col-10">
            <input
              type="date"
              max={new Date().toISOString().slice(0, 10)}
              id="installedDate"
              className={"form-control data-md-disable-future"}
              name="installedDate"
              value={formik.values.installedDate}
              onChange={formik.handleChange}
            />
            {formik.errors.installedDate && (
              <p className="text-danger mb-0">{formik.errors.installedDate}</p>
            )}
          </div>
        </div>
        {/* State */}
        <div className="row mb-3">
          <div className="col-2">
            <label htmlFor="state" className="col-form-label">
              State
            </label>
          </div>
          <div className="state-checkbox-container col-10">
            <div className="form-check">
              <label className="form-check-label" htmlFor="Available">
                Available
              </label>
              <input
                type="radio"
                id="Available"
                className="form-check-input"
                name="state"
                value="Available"
                checked={formik.values.state === "Available" ? true : false}
                onChange={formik.handleChange}
              />
            </div>
            <div className="form-check">
              <label className="form-check-label" htmlFor="NotAvailable">
                Not available
              </label>
              <input
                type="radio"
                id="NotAvailable"
                className="form-check-input"
                name="state"
                value="NotAvailable"
                checked={formik.values.state === "NotAvailable" ? true : false}
                onChange={formik.handleChange}
              />
            </div>
            <div className="form-check">
              <label className="form-check-label" htmlFor="WaitingForRecycling">
                Waiting For Recycling
              </label>
              <input
                type="radio"
                id="WaitingForRecycling"
                className="form-check-input"
                name="state"
                value="WaitingForRecycling"
                checked={
                  formik.values.state === "WaitingForRecycling" ? true : false
                }
                onChange={formik.handleChange}
              />
            </div>
            <div className="form-check">
              <label className="form-check-label" htmlFor="Recycled">
                Recycled
              </label>
              <input
                type="radio"
                id="Recycled"
                className="form-check-input"
                name="state"
                value="Recycled"
                checked={formik.values.state === "Recycled" ? true : false}
                onChange={formik.handleChange}
              />
            </div>

            {formik.errors.state && formik.touched.state && (
              <p className="text-danger mb-0">{formik.errors.state}</p>
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
          <p> Do you want to cancel edit asset?</p>
          <div className="modal-confirm-button-container">
            <button
              className="btn confirm-accept-button"
              style={{ width: "13%" }}
              onClick={() => navigate(`/assets`)}
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
};

export default UpdateAsset;
