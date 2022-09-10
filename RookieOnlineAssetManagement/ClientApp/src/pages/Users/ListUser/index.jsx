import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axiosInstance from "../../../axios";
import moment from "moment";

import DisableUser from "../DisableUser";
import ModalExample from "../../../components/Modal/Modal";

import { FilterOutlined } from "@ant-design/icons";
import { Table, Typography, Dropdown, Menu, Input, Button } from "antd";
import "antd/dist/antd.css";
import "./index.css";

const UserTable = () => {
  const state = localStorage.getItem("identifier", "userCreated");
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const [menuType, setMenuType] = useState("Type");
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    type: "All",
    keyword: "",
  });
  const fetchData = (params = {}) => {
    setLoading(true);
    axiosInstance
      .get(
        `users/GetList?&page=${params.pagination.current}&pageSize=${
          params.pagination.pageSize
        }&keyword=${params.pagination.keyword}&types=${
          params.pagination.type
        }&sortOrder=${state ? "descend" : params.sortOrder}&sortField=${
          state ? "staffCode" : params.sortField
        }`
      )
      .then((results) => {
        results.users.forEach((element) => {
          element.joinedDate = moment(element.joinedDate).format("DD/MM/YYYY");
        });
        setData(results.users);
        setLoading(false);
        setPagination({
          ...params.pagination,
          total: results.totalItem,
        });
      });
  };

  const columns = [
    {
      title: "Staff Code",
      dataIndex: "staffCode",
      width: "10%",
      sorter: true,
    },
    {
      title: "Full Name",
      dataIndex: "fullName",
      ellipsis: true,
      defaultSortOrder: state ? null : "ascend",
      sorter: true,
    },
    {
      title: "UserName",
      dataIndex: "userName",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Joined Date",
      width: "15%",
      dataIndex: "joinedDate",
      sorter: true,
    },
    {
      title: "Type",
      width: "10%",
      dataIndex: "type",
    },
    {
      title: "Action",
      dataIndex: "id",
      key: "id",
      width: "10%",
      render: (id) => (
        <div className="user-button-group">
          <i
            className="fa-solid fa-edit fa-lg user-edit-button"
            onClick={() => {
              navigate(`update/${id}`);
            }}
          ></i>
          <i
            className="fa-solid fa-xmark fa-xl user-delete-button"
            onClick={(e) => {
              e.stopPropagation();
              showSubModal(id);
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

  const showModal = (e) => {
    setIsModalVisible(true);
    setInfor(e);
  };

  const showSubModal = (id) => {
    setBool(true);
    setId(id);
  };

  const handleCancel = () => {
    setIsModalVisible(false);
    setBool(false);
  };

  const { Search } = Input;

  useEffect(() => {
    fetchData({
      pagination,
    });
    if (state) {
      if (performance.navigation.type === performance.navigation.TYPE_RELOAD) {
        localStorage.removeItem("identifier");
      }
    }
  }, []);

  const handleMenuClick = (e) => {
    setPagination((pagination.type = e.key));
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
          label: "Admin",
          key: "Admin",
        },
        {
          label: "Staff",
          key: "Staff",
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

  return (
    <div className="user-table">
      <Typography className="header-user-list">User List</Typography>
      <Dropdown
        overlay={menu}
        placement="bottom"        
      >
        <Button className="btn-filter" style={{marginTop: "0.5rem",}}>
            <span></span>
            {menuType}
            <FilterOutlined />
        </Button>
      </Dropdown>
      <Button
        className="create-user-button"
        type="primary"
        onClick={() => navigate("/users/add")}
      >
        Create new user
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
      <ModalExample
        visible={isModalVisible}
        handleCancel={handleCancel}
        data={infor}
      />
      <DisableUser id={id} bool={bool} handleCancel={handleCancel} />
    </div>
  );
};

export default UserTable;
