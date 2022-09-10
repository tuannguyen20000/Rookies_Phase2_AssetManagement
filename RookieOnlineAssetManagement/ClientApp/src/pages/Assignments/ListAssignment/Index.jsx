import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../axios";
import moment from "moment";

import ModalAssignment from "./Modal/Modal";
import DeleteAssignmentModal from "../DeleteAssignment";
import ReturnAssignmentModal from "../../../components/Modal/ModalReturn";

import { FilterOutlined } from "@ant-design/icons";
import {
  Table,
  Typography,
  Dropdown,
  Menu,
  Input,
  Button,
  DatePicker,
} from "antd";
import "antd/dist/antd.css";
import "./index.css";

const AssignmentTable = () => {
  let state = localStorage.getItem("AssignmentIdentifier", "assignmentCreated");
  let updatedAssignmentId = localStorage.getItem("AssignmentIdUpdated");
  let currentPageReturn = localStorage.getItem("ReturnCurrentPage");
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const [menuType, setMenuType] = useState("State");
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    state: "All",
    keyword: "",
    assignedDate: "0001-01-01",
  });

  const fetchData = (params = {}) => {
    setLoading(true);
    axiosInstance
      .get(
        `assignment/getlist-assignment?&page=${
          params.pagination.current
        }&pageSize=${params.pagination.pageSize}&keyword=${
          params.pagination.keyword
        }&assignedDate=${params.pagination.assignedDate}&states=${
          params.pagination.state
        }&sortOrder=${state ? "descend" : params.sortOrder}&sortField=${
          state ? "id" : params.sortField
        }`
      )
      .then((results) => {
        let dataAssignment = [];
        results.assignments.map((assignment) => {
          if (updatedAssignmentId && +updatedAssignmentId === assignment.id) {
            dataAssignment.unshift({
              id: assignment.id,
              assetCode: assignment.assetCode,
              assetName: assignment.assetName,
              assignedTo: assignment.assignedTo,
              assignedBy: assignment.assignedBy,
              specification: assignment.specification,
              note: assignment.note,
              state:
                assignment.state === "WaitingForAcceptance"
                  ? "Waiting For Acceptance"
                  : assignment.state,
              assignedDate: moment(assignment.assignedDate).format(
                "DD/MM/YYYY"
              ),
              requestState:
                assignment.requestState === "WaitingForReturning"
                  ? "Waiting For Returning"
                  : assignment.requestState,
              requestedById: assignment.requestedById,
            });
          } else {
            dataAssignment.push({
              id: assignment.id,
              assetCode: assignment.assetCode,
              assetName: assignment.assetName,
              assignedTo: assignment.assignedTo,
              assignedBy: assignment.assignedBy,
              specification: assignment.specification,
              note: assignment.note,
              state:
                assignment.state === "WaitingForAcceptance"
                  ? "Waiting For Acceptance"
                  : assignment.state,
              assignedDate: moment(assignment.assignedDate).format(
                "DD/MM/YYYY"
              ),
              requestState:
                assignment.requestState === "WaitingForReturning"
                  ? "Waiting For Returning"
                  : assignment.requestState,
              requestedById: assignment.requestedById,
            });
          }
        });

        setData(dataAssignment);
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
    if (state || updatedAssignmentId) {
      if (performance.navigation.type === performance.navigation.TYPE_RELOAD) {
        localStorage.removeItem("AssignmentIdUpdated");
        localStorage.removeItem("ReturnCurrentPage");
        localStorage.removeItem("AssignmentIdentifier");
      }
    }
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
      title: "Assigned to",
      dataIndex: "assignedTo",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Assigned by",
      dataIndex: "assignedBy",
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
      title: "State",
      dataIndex: "state",
      width: "20%",
      sorter: true,
    },
    {
      title: "Action",
      dataIndex: "id",
      key: "id",
      width: "12%",
      render: (id, record) =>
        record.state === "Waiting For Acceptance" ? (
          <div className="assignment-button-group">
            <i
              className="fa-solid fa-edit fa-lg assignment-edit-button"
              onClick={() => {
                localStorage.setItem("ReturnCurrentPage", pagination.current);
                navigate(`update/${id}`);
              }}
            ></i>
            <i
              className="fa-solid fa-xmark fa-xl assignment-delete-button"
              onClick={(e) => {
                e.stopPropagation();
                showSubModal(id);
              }}
            ></i>
            <i className="fa-solid fa-rotate-left fa-lg assignment-return-button-disabled"></i>
          </div>
        ) : record.requestState === "Waiting For Returning" ? (
          <div className="assignment-button-group">
            <i className="fa-solid fa-edit fa-lg assignment-edit-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl assignment-delete-button-disabled"></i>
            <i className="fa-solid fa-rotate-left fa-lg assignment-return-button-disabled"></i>
          </div>
        ) : (
          <div className="assignment-button-group">
            <i className="fa-solid fa-edit fa-lg assignment-edit-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl assignment-delete-button-disabled"></i>
            <i
              className="fa-solid fa-rotate-left fa-lg assignment-return-button"
              
              onClick={(e) => {
                e.stopPropagation();
                showReturnModal(id);
              }}
            ></i>
          </div>
        ),
    },
  ];

  const [isModalVisible, setIsModalVisible] = useState(false);
  const [bool, setBool] = useState(false);
  const [infor, setInfor] = useState();
  const [id, setId] = useState();

  const [isModalVisibleReturn, setIsModalVisibleReturn] = useState(false);
  const [boolReturn, setBoolReturn] = useState(false);
  const showModal = (e) => {
    setIsModalVisible(true);
    setInfor(e);
  };

  const showSubModal = (id) => {
    setBool(true);
    setId(id);
  };

  const showReturnModal = (id) => {
    setBoolReturn(true);
    setId(id);
  };
  const handleDelete = (id) => {
    const newData = data.filter((item) => item.id !== id);
    setData(newData);
    fetchData({
      pagination,
    });
  };

  const handleReturn = (id) => {
    const newData = data.filter((item) => item.id !== id);
    setData(newData);
    fetchData({
      pagination,
    });
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    setBool(false);
  };

  const handleCancelReturn = () => {
    setIsModalVisibleReturn(false);
    setBoolReturn(false);
  };

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
          label: "Accepted",
          key: "Accepted",
        },
        {
          label: "Waiting for acceptance",
          key: "WaitingForAcceptance",
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
      setPagination((pagination.assignedDate = "0001-01-01"));
      fetchData({
        pagination,
      });
    } else {
      setPagination(
        (pagination.assignedDate = moment(value).format("YYYY-MM-DD"))
      );
      fetchData({
        pagination,
      });
    }
  };

  return (
    <div className="user-table">
      <Typography className="header-user-list">Assignment List</Typography>
      <div className="filter-group">
          <Dropdown
            overlay={menu}
            placement="bottom"
          >
            <Button className="btn-filter" style={{ marginTop: "0.5rem", }}>
              <span></span>
              {menuType}
              <FilterOutlined />
            </Button>
          </Dropdown>
          <DatePicker
            inputReadOnly={true}
            style={{
              marginTop: "0.5rem",
              marginLeft: "1rem",
            }}
            onChange={handleAssignedDate}
          />
      </div>
      <Button
        className="create-user-button"
        type="primary"
        onClick={() => navigate("/assignments/add")}
      >
        Create new assignment
      </Button>
      <Search onSearch={onSearch} className="search-box" />

      <Table
        columns={columns}
        dataSource={data}
        rowKey={(record) => record.id}
        onRow={(record, rowIndex) => {
          return {
            onClick: () => {
              showModal(record);
            },
          };
        }}
        pagination={pagination}
        loading={loading}
        onChange={onChange}
      />
      <ModalAssignment
        visible={isModalVisible}
        handleCancel={handleCancel}
        data={infor}
      />
      <DeleteAssignmentModal
        assignmentId={id}
        bool={bool}
        handleDelete={handleDelete}
        handleCancel={handleCancel}
      />
      <ReturnAssignmentModal
        assignmentId={id}
        boolReturn={boolReturn}
        handleReturn={handleReturn}
        handleCancelReturn={handleCancelReturn}
      />
    </div>
  );
};

export default AssignmentTable;
