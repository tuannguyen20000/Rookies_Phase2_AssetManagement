import { React, useState, useEffect, useRef } from "react";
import { useFormik } from "formik";
import { useNavigate } from "react-router-dom";
import * as yup from "yup";

import assetService from "../../../services/assetService";
import Modal from "react-bootstrap/Modal";

import "./CreateAsset.css";

const CreateAsset = () => {
  const navigate = useNavigate();
  const [formState, setFormState] = useState(true);
  const [newCategoryError, setNewCategoryError] = useState([]);
  const [categoryValue, setCategoryValue] = useState("");
  const [categoryId, setCategoryId] = useState(null);
  const [categoryError, setCategoryError] = useState(false);
  const [categories, setCategories] = useState([]);
  const [cancel, setCancel] = useState(false);
  const newCategoryNameRef = useRef();
  const newCategoryPrefixRef = useRef();

  const fetchData = () => {
    assetService.getCategories().then((result) => {
      setCategories(result.data);
    });
  };

  useEffect(() => {
    fetchData();
  }, []);

  const formik = useFormik({
    initialValues: {
      assetName: "",
      categoryId: "",
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
      setCategoryError(false);
      if (categoryId !== null) {
        data.categoryId = categoryId;
        assetService
          .createAsset(data)
          .then((response) => {
            localStorage.setItem("assetIdentifier", "assetModify");
            localStorage.setItem("AssetIdCreated", response.data.id);
            navigate("/assets");
          })
          .catch((e) => {
            console.log(e);
          });
      } else {
        setCategoryError(true);
      }
    },
  });

  const onAddNewCategoryClick = () => {
    setFormState((current) => !current);
  };

  const handleCancel = () => {
    setCancel(true);
  };

  const handleReturn = () => {
    setCancel(false);
  };

  const onNewCategorySubmit = () => {
    let checkName = true;
    let checkPrefix = true;
    const data = {
      categoryName: newCategoryNameRef.current.value,
      categoryPrefix: newCategoryPrefixRef.current.value,
    };
    const uppercaseRegExp = /^[A-Z]*$/;
    const lengthRegExp = /(?=.{2}$)/;
    const uppercasePrefix = uppercaseRegExp.test(data.categoryPrefix);
    const lengthPrefix = lengthRegExp.test(data.categoryPrefix);
    setNewCategoryError([]);

    categories.map((category) => {
      if (data.categoryName === category.categoryName) checkName = false;
      if (data.categoryPrefix === category.categoryPrefix) checkPrefix = false;
    });
    if (checkName && checkPrefix && uppercasePrefix && lengthPrefix) {
      setFormState((current) => !current);
      // add data
      assetService.createCategory(data).then((result) => {
        fetchData();
      });
    } else {
      if (!checkName)
        setNewCategoryError((error) => [
          ...error,
          "Category is already existed. Please enter a different category",
        ]);
      if (!checkPrefix)
        setNewCategoryError((error) => [
          ...error,
          "Prefix is already existed. Please enter a different prefix",
        ]);
      if (!uppercasePrefix)
        setNewCategoryError((error) => [
          ...error,
          "Prefix must be uppercase. Please enter a new prefix",
        ]);
      if (!lengthPrefix)
        setNewCategoryError((error) => [
          ...error,
          "Prefix must have 2 characters. Please enter a new prefix",
        ]);
    }
  };

  return (
    <div className="asset-container">
      <h2 className="text-danger fw-bold mb-4" style={{ fontSize: 24 }}>
        Create New Asset
      </h2>
      <form
        className="form-create needs-validation"
        onSubmit={formik.handleSubmit}
      >
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
        <div className="row mb-3">
          <div className="col-2">
            <label htmlFor="category" className="col-form-label">
              Category
            </label>
          </div>
          <div className="col-10">
            <div>
              <div
                className="h-100 create-category-dropdown form-control"
                data-bs-toggle="dropdown"
                aria-expanded="false"
              >
                <p>
                  {categoryValue !== ""
                    ? categoryValue
                    : "Select asset category"}
                </p>
                <div className="d-flex align-items-center">
                  <i className="fa-solid fa-sort-down"></i>
                </div>
              </div>
              <ul className="dropdown-menu">
                {categories.map((category) => (
                  <li>
                    <div
                      className="dropdown-item"
                      onClick={() => {
                        setCategoryValue(category.categoryName);
                        setCategoryId(category.id);
                      }}
                    >
                      {category.categoryName}
                    </div>
                  </li>
                ))}
                <li
                  className="dropdown-item-bottom"
                  onClick={(e) => {
                    e.stopPropagation();
                  }}
                >
                  <div
                    className="dropdown-item add-new-category"
                    hidden={!formState}
                    onClick={onAddNewCategoryClick}
                  >
                    Add new category
                  </div>
                  <div className="add-new-category-form" hidden={formState}>
                    <input
                      name="new-category-name"
                      className="name"
                      type="text"
                      placeholder="Enter new category name"
                      ref={newCategoryNameRef}
                    />
                    <input
                      name="new-category-prefix"
                      className="prefix"
                      type="text"
                      placeholder="Enter new category prefix"
                      maxlength="2"
                      ref={newCategoryPrefixRef}
                    />
                    <div
                      className="submit-button"
                      onClick={onNewCategorySubmit}
                    >
                      <i className="fa-solid fa-check"></i>
                    </div>
                    <div
                      className="cancel-button"
                      onClick={onAddNewCategoryClick}
                    >
                      <i className="fa-solid fa-xmark"></i>
                    </div>
                  </div>
                  <div>
                    <ul>
                      {newCategoryError.map((errors) => (
                        <li className="add-new-category-error">{errors}</li>
                      ))}
                    </ul>
                  </div>
                </li>
              </ul>
            </div>
            {categoryError ? (
              <p className="text-danger mb-0">Asset category is required</p>
            ) : (
              <div></div>
            )}
          </div>
        </div>
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
              className={`form-control ${
                formik.errors.installedDate &&
                formik.touched.installedDate === true
                  ? "is-invalid"
                  : ""
              }`}
              name="installedDate"
              value={formik.values.installedDate}
              onChange={formik.handleChange}
            />
            {formik.errors.installedDate && formik.touched.installedDate && (
              <p className="text-danger mb-0">{formik.errors.installedDate}</p>
            )}
          </div>
        </div>
        <div className="row mb-3">
          <div className="col-2">
            <label htmlFor="state" className="col-form-label">
              State
            </label>
          </div>
          <div className="state-checkbox-container col-10">
            <div className="form-check">
              <label className="form-check-label" htmlFor="Female">
                Available
              </label>
              <input
                type="radio"
                id="Available"
                className={`form-check-input ${
                  formik.errors.state && formik.touched.state === true
                    ? "is-invalid"
                    : ""
                }`}
                name="state"
                value="1"
                onChange={formik.handleChange}
              />
            </div>
            <div className="form-check">
              <label className="form-check-label" htmlFor="Male">
                Not available
              </label>
              <input
                type="radio"
                id="NotAvailable"
                className={`form-check-input ${
                  formik.errors.state && formik.touched.state === true
                    ? "is-invalid"
                    : ""
                }`}
                name="state"
                value="2"
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
            className={`btn save-button ${
              formik.values.name !== "" &&
              categoryId !== null &&
              formik.values.specification !== "" &&
              formik.values.installedDate !== "" &&
              formik.values.state !== ""
                ? ""
                : "disabled"
            }`}
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
          <p> Do you want to cancel create asset?</p>
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

export default CreateAsset;
