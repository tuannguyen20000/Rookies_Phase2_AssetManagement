import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../axios";
import moment from "moment";

import assetService from "../../../services/assetService";
import ModalAsset from "../../../components/Modal/ModalAsset";
import DeleteAsset from "../DeleteAsset/DeleteAsset";

import { Button, Input, Table, Dropdown, Checkbox, Typography } from "antd";
import { FilterOutlined } from "@ant-design/icons";
import "antd/dist/antd.css";
import "./style.css";

const defaultCheckedStateList = ["Available", "NotAvailable", "Assigned"];

function Index() {
  let stateModify = localStorage.getItem("assetIdentifier", "assetModify");
  let updatedAssetId = localStorage.getItem("AssetIdUpdated");
  let createdAssetId = localStorage.getItem("AssetIdCreated");
  const navigate = useNavigate();
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState([]);
  const [infor, setInfor] = useState();
  const [checkedStateList, setCheckedStateList] = useState(
    defaultCheckedStateList
  );
  const [stateIndeterminate, setStateIndeterminate] = useState(true);
  const [stateCheckAll, setStateCheckAll] = useState(false);
  // modal
  const [isModalVisible, setIsModalVisible] = useState(false);
  const { Search } = Input;
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    keyword: "",
    categories: "All",
    states: "All",
  });
  const plainOptions = [
    { value: "Available", label: "Available" },
    { value: "NotAvailable", label: "Not Available" },
    { value: "Assigned", label: "Assigned" },
    { value: "WaitingForRecycling", label: "Waiting For Recycling" },
    { value: "Recycled", label: "Recycled" },
  ];

  const fetchData = (params = {}) => {
    var stateParams;
    stateParams =
      params.pagination.states === "All"
        ? ""
        : Array.from(params.pagination.states).reduce(
            (init, state) => init + `&states=${state}`,
            ""
          );

    var categoriesParams;
    if (params.pagination.categories === "All") {
      categoriesParams = "";
    } else {
      categoriesParams = Array.from(params.pagination.categories).reduce(
        (init, category) => init + `&categories=${category}`,
        ""
      );
    }
    setLoading(true);
    axiosInstance
      .get(
        `assets/asset-list?&page=${params.pagination.current}&pageSize=${
          params.pagination.pageSize
        }&keyword=${
          params.pagination.keyword
        }${stateParams}${categoriesParams}&sortOrder=${
          stateModify ? "descend" : params.sortOrder
        }&sortField=${stateModify ? "id" : params.sortField}`
      )
      .then((results) => {
        let dataAsset = [];
        results.assets.map((asset) => {
          if (
            (updatedAssetId && +updatedAssetId === asset.id) ||
            (createdAssetId && +createdAssetId === asset.id)
          ) {
            dataAsset.unshift({
              id: asset.id,
              assetCode: asset.assetCode,
              assetName: asset.assetName,
              category: asset.category,
              location: asset.location,
              specification: asset.specification,
              state:
                asset.state === "WaitingForAcceptance"
                  ? "Waiting For Acceptance"
                  : asset.state,
              installedDate: moment(asset.installedDate).format("DD/MM/YYYY"),
            });
          } else {
            dataAsset.push({
              id: asset.id,
              assetCode: asset.assetCode,
              assetName: asset.assetName,
              category: asset.category,
              location: asset.location,
              specification: asset.specification,
              state:
                asset.state === "WaitingForAcceptance"
                  ? "Waiting For Acceptance"
                  : asset.state,
              installedDate: moment(asset.installedDate).format("DD/MM/YYYY"),
            });
          }
          //End else
        });
        setData(dataAsset);
        setLoading(false);
        setPagination({
          ...params.pagination,
          total: results.totalItem,
        });
      });
  };

  const columns = [
    {
      title: "Asset Code",
      dataIndex: "assetCode",
      key: "assetCode",
      width: "10%",
      sorter: true,
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      key: "assetName",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Category",
      dataIndex: "category",
      key: "category",
      ellipsis: true,
      width: "15%",
      sorter: true,
    },
    {
      title: "State",
      dataIndex: "state",
      key: "state",
      width: "23%",
      sorter: true,
    },
    {
      title: "Action",
      dataIndex: "id",
      key: "id",
      width: "10%",
      render: (id, record) =>
        record.state === "Assigned" ? (
          <div className="asset-button-group">
            <i className="fa-solid fa-edit fa-lg asset-edit-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl asset-delete-button-disabled"></i>
          </div>
        ) : (
          <div className="asset-button-group">
            <i
              className="fa-solid fa-edit fa-lg asset-edit-button"
              onClick={() => {
                navigate(`update/${id}`);
              }}
            ></i>
            <i
              className="fa-solid fa-xmark fa-xl asset-delete-button"
              onClick={(e) => {
                e.stopPropagation();
                showSubModal(id);
              }}
            ></i>
          </div>
        ),
    },
  ];

  useEffect(() => {
    assetService
      .getCategories()
      .then((response) => {
        var temp = response.data.map((item) => item.categoryName);
        setCategories(temp);
      })
      .catch((error) => console.log(error));
    fetchData({
      pagination,
    });
    if (stateModify || updatedAssetId) {
      if (performance.navigation.type === performance.navigation.TYPE_RELOAD) {
        localStorage.removeItem("assetIdentifier");
        localStorage.removeItem("AssetIdUpdated");
        localStorage.removeItem("AssetIdCreated");
        console.info("This page is reloaded");
      }
    }
  }, []);

  const [bool, setBool] = useState(false);
  const [id, setId] = useState();

  const showSubModal = (id) => {
    setBool(true);
    setId(id);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    setBool(false);
  };

  const [history, setHistory] = useState({});
  const ShowModal = (e) => {
    setIsModalVisible(true);
    setInfor(e);
    assetService.getHistory(e.id).then((res) => setHistory(res.data));
    console.log(history);
  };

  const onSearch = (value) => {
    setPagination((pagination.keyword = value));
    fetchData({
      pagination,
    });
  };

  const onChangeCategories = (value) => {
    setPagination((pagination.categories = value));
    fetchData({
      pagination,
    });
  };

  const onChangeState = (value) => {
    setCheckedStateList(value);
    setStateIndeterminate(!!value.length && value.length < plainOptions.length);
    setStateCheckAll(value.length === plainOptions.length);
    setPagination((pagination.states = value));
    fetchData({
      pagination,
    });
    if (value.length === 0) {
      setCheckedStateList(defaultCheckedStateList);
      setPagination((pagination.states = defaultCheckedStateList));
      fetchData({
        pagination,
      });
    }
  };

  const onCheckAllChangeState = (e) => {
    var temp = Array.from(plainOptions).map((option) => option.value);
    var check = e.target.checked ? temp : defaultCheckedStateList;
    setCheckedStateList(check);
    setStateIndeterminate(false);
    setStateCheckAll(e.target.checked);
    setPagination((pagination.states = check));
    fetchData({
      pagination,
    });
  };

  const menu = (
    <div
      style={{
        width: "100%",
        background: "#f5f5f5",
        border: "1px solid #d8d8d8",
        padding: ".5rem 1rem",
      }}
    >
      <Checkbox
        indeterminate={stateIndeterminate}
        onChange={onCheckAllChangeState}
        checked={stateCheckAll}
      >
        All
      </Checkbox>
      <Checkbox.Group
        options={plainOptions}
        value={checkedStateList}
        onChange={onChangeState}
      >
        {plainOptions.map((option, ind) => (
          <Checkbox key={ind} value={option.value}>
            {option.label}
          </Checkbox>
        ))}
      </Checkbox.Group>
    </div>
  );

  const menuCategories = (
    <div
      style={{
        width: "100%",
        background: "#f5f5f5",
        border: "1px solid #d8d8d8",
        padding: ".5rem 1rem",
      }}
    >
      <Checkbox.Group onChange={onChangeCategories}>
        <Checkbox value="All" checked>
          All
        </Checkbox>
        {categories.map((category, ind) => (
          <Checkbox key={ind} value={category}>
            {category}
          </Checkbox>
        ))}
      </Checkbox.Group>
    </div>
  );

  const onChange = (newPagination, filters, sorter, extra) => {
    fetchData({
      pagination: newPagination,
      sortField: sorter.field,
      sortOrder: sorter.order,
      ...filters,
    });
  };

  return (
    <div className="asset-table">
      <Typography className="header-user-list">Asset List</Typography>
      <div className="asset-table-header-filter">
        <Dropdown
          overlay={menu}
          placement="bottomRight"
          className="asset-button-states"
          id="states-filter"
        >
          <Button className="asset-icon-filter">
            <span></span>
            State
            <FilterOutlined />
          </Button>
        </Dropdown>
        <Dropdown
          overlay={menuCategories}
          placement="bottomRight"
          className="asset-button-categories"
          id="categories-filter"
        >
          <Button className="asset-icon-categories">
            <span></span>
            Categories
            <FilterOutlined />
          </Button>
        </Dropdown>
        <Button
          className="create-asset-button"
          type="primary"
          onClick={() => navigate("/assets/add")}
        >
          Create new asset
        </Button>
        <Search
          placeholder=""
          allowClear
          onSearch={onSearch}
          style={{ width: 200 }}
          className="asset-input-search"
        />
      </div>
      <Table
        rowKey={(record) => record.id}
        dataSource={data}
        columns={columns}
        onRow={(record, rowIndex) => {
          return {
            onClick: () => {
              ShowModal(record);
            },
          };
        }}
        pagination={pagination}
        loading={loading}
        onChange={onChange}
      />
      <ModalAsset
        visible={isModalVisible}
        handleCancel={handleCancel}
        data={infor}
        history={history}
      />
      <DeleteAsset id={id} bool={bool} handleCancel={handleCancel} />
    </div>
  );
}

export default Index;
