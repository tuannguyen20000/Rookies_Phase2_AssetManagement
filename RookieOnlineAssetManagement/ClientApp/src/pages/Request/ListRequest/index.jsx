import React, { useEffect, useState } from "react";
import moment from "moment";
import requestService from "../../../services/requestService";
import { useNavigate } from "react-router-dom";
import Modal from "react-bootstrap/Modal";
import "./index.css";
import { FilterOutlined } from "@ant-design/icons";
import { DatePicker, Dropdown, Input, Menu, Table, Typography, Button } from "antd";
import "antd/dist/antd.css";
import axiosInstance from "../../../axios";

function Index() {
  const [showComplete, setShowComplete] = useState(false);
  const [showCancel, setShowCancel] = useState(false);
  const [id, setId] = useState();
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const [menuType, setMenuType] = useState("State");
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    states: "All",
    keyword: "",
    returnedDate: "0001-01-01",
  });

  const fetchData = (params = {}) => {
    setLoading(true);
    axiosInstance
      .get(
        `requests/get-requests?&page=${params.pagination.current}&pageSize=${
          params.pagination.pageSize
        }&keyword=${params.pagination.keyword}&returnedDate=${
          params.pagination.returnedDate
        }&states=${
          params.pagination.state ? params.pagination.state : "All"
        }&sortOrder=${
          params.sortOrder ? params.sortOrder : "ascend"
        }&sortField=${params.sortField ? params.sortField : "assetCode"}`
      )
      .then((results) => {
        results.requests.map((request) => {
          request.assignedDate = moment(request.assignedDate).format(
            "MM/DD/YYYY"
          );
          request.returnedDate = moment(request.returnedDate).format(
            "MM/DD/YYYY"
          );
          request.requestState =
            request.requestState === "WaitingForReturning"
              ? "Waiting For Returning"
              : "Completed";
        });
        setData(results.requests);
        console.log(results.requests);
        setLoading(false);
        setPagination({
          ...params.pagination,
          total: results.totalItem,
        });
      });
  };

  useEffect(() => {
    fetchData({
      pagination,
    });
  }, []);

  const columns = [
    {
      title: "No.",
      dataIndex: "index",
      width: "5%",
      render: (value, item, index) =>
        pagination.current === 1
          ? index + 1
          : (pagination.current - 1) * pagination.pageSize + (index + 1),
    },
    {
      title: "Asset Code",
      dataIndex: "assetCode",
      width: "10%",
      sorter: true,
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Requested by",
      dataIndex: "requestedBy",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Assigned Date",
      dataIndex: "assignedDate",
      width: "13%",
      sorter: true,
    },
    {
      title: "Accepted By",
      dataIndex: "acceptedBy",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Returned Date",
      dataIndex: "returnedDate",
      width: "13%",
      sorter: true,
      render: (id, record) =>
        record.requestState === "Waiting For Returning" ? (
          <div></div>
        ) : (
          <div>{record.returnedDate}</div>
        ),
    },
    {
      title: "State",
      dataIndex: "requestState",
      width: "15%",
      sorter: true,
    },
    {
      title: "Action",
      dataIndex: "id",
      key: "id",
      width: "10%",
      render: (id, record) =>
        record.requestState === "Waiting For Returning" ? (
          <div className="request-list-button-group">
            <i
              className="fa-solid fa-check fa-xl request-list-accept-button"
              onClick={(e) => {
                e.stopPropagation();
                setShowComplete(true);
                setId(id);
              }}
            ></i>
            <i
              className="fa-solid fa-xmark fa-xl request-list-cancel-button"
              onClick={(e) => {
                e.stopPropagation();
                setShowCancel(true);
                setId(id);
              }}
            ></i>
          </div>
        ) : (
          <div className="request-list-button-group">
            <i className="fa-solid fa-check fa-xl request-list-accept-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl request-list-cancel-button-disabled"></i>
          </div>
        ),
    },
  ];

  const { Search } = Input;

  const handleMenuClick = (e) => {
    setPagination((pagination.state = e.key));
    setMenuType(e.key);
    fetchData({
      pagination,
    });
  };

  const onSearch = (value) => {
    setPagination((pagination.keyword = value), (pagination.current = 1));
    fetchData({
      pagination,
    });
  };

  const menu = (
    <Menu
      onClick={handleMenuClick}
      items={[
        {
          label: "All",
          key: "All",
        },
        {
          label: "Completed",
          key: "Completed",
        },
        {
          label: "Waiting for returning",
          key: "WaitingForReturning",
        },
      ]}
    />
  );

  const onChange = (newPagination, filters, sorter, extra) => {
    fetchData({
      sortField: sorter.field,
      sortOrder: sorter.order,
      pagination: newPagination,
      ...filters,
    });
  };

  const handleAssignedDate = (value) => {
    if (moment(value).format("YYYY-MM-DD") === "Invalid date") {
      setPagination((pagination.returnedDate = "0001-01-01"));
      fetchData({
        pagination,
      });
    } else {
      setPagination(
        (pagination.returnedDate = moment(value).format("YYYY-MM-DD"))
      );
      fetchData({
        pagination,
      });
    }
  };

  const handleComplete = () => {
    requestService.completeRequest(id).then(() => {
      setId(0);
      setShowComplete(false);
      window.location.reload();
    });
  };

  const handleCancel = () => {
    requestService.cancelRequest(id).then(() => {
      setId(0);
      setShowCancel(false);
      window.location.reload();
    });
  };

  return (
    <div className="user-table" id="request-table">
      <Typography className="header-user-list">Request List</Typography>
      <div className="filter-group" style={{ marginTop: "0.5rem", }}>
          <Dropdown
            overlay={menu}
            placement="bottom"
          >
            <Button className="btn-filter">
              <span></span>
              {menuType}
              <FilterOutlined />
            </Button>
          </Dropdown>
          <DatePicker
            inputReadOnly={true}
            style={{
              marginLeft: "1rem",
            }}
            onChange={handleAssignedDate}
          />
      </div>
      <Search onSearch={onSearch} className="search-box" />

      <Table
        columns={columns}
        dataSource={data}
        rowKey={(record) => record.id}
        onRow={(record, rowIndex) => {
          return {
            onClick: () => {
              console.log(record);
            },
          };
        }}
        pagination={pagination}
        loading={loading}
        onChange={onChange}
      />
      <div className="request-modal">
        <Modal
          show={showComplete}
          onHide={handleComplete}
          backdrop="static"
          keyboard={false}
          id="request-complete-modal"
        >
          <Modal.Header>
            <Modal.Title>
              <p>Are you sure?</p>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p> Do you want to mark this returning request as 'Completed'?</p>
            <div>
              <button
                className="btn request-accept-button"
                style={{ width: "13%" }}
                onClick={handleComplete}
              >
                Yes
              </button>
              <button
                className="btn request-cancel-button"
                style={{ width: "13%" }}
                onClick={() => setShowComplete(false)}
              >
                No
              </button>
            </div>
          </Modal.Body>
        </Modal>
      </div>
      <div className="request-modal">
        <Modal
          show={showCancel}
          onHide={handleCancel}
          backdrop="static"
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>
              <p>Are you sure?</p>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p> Do you want to cancel this returning request?</p>
            <div>
              <button
                className="btn request-accept-button"
                style={{ width: "13%" }}
                onClick={handleCancel}
              >
                Yes
              </button>
              <button
                className="btn request-cancel-button"
                style={{ width: "13%" }}
                onClick={() => setShowCancel(false)}
              >
                No
              </button>
            </div>
          </Modal.Body>
        </Modal>
      </div>
    </div>
  );
}

export default Index;
