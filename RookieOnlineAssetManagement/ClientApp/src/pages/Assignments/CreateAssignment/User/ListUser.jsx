import React, { useEffect, useState } from "react";
import axiosInstance from "../../../../axios";
import { Table, Typography, Input, Divider } from "antd";

import "antd/dist/antd.css";

const ListUser = (props) => {
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);

  const rowSelection = {
    selectedRowKeys: selectedRowKeys,
    onChange: (selectedRowKeys, selectedRows) => {
      setSelectedRowKeys(selectedRowKeys);
      var userId = selectedRowKeys[0];
      var fullName = selectedRows[0].fullName;
      var infoUser = {
        id: userId,
        fullName: fullName,
      };
      props.onSelectedUser(infoUser);
    },
  };

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
        `users/GetList?&page=${params.pagination.current}&pageSize=${params.pagination.pageSize}&keyword=${params.pagination.keyword}&types=${params.pagination.type}&sortOrder=${params.sortOrder}&sortField=${params.sortField}`
      )
      .then((results) => {
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
      width: "20%",
      sorter: true,
    },
    {
      title: "Full Name",
      dataIndex: "fullName",
      ellipsis: true,
      defaultSortOrder: "ascend",
      sorter: true,
    },
    {
      title: "Type",
      dataIndex: "type",
      width: "20%",
      sorter: true,
    },
  ];

  const { Search } = Input;

  useEffect(() => {
    fetchData({
      pagination,
    });
  }, []);

  const onSearch = (value) => {
    setPagination((pagination.keyword = value));
    fetchData({
      pagination,
    });
  };
  const onChange = (newPagination, filters, sorter, extra) => {
    fetchData({
      sortField: sorter.field,
      sortOrder: sorter.order,
      pagination: newPagination,
      ...filters,
    });
  };

  return (
    <div>
      <div style={{ display: "flex" }}>
        <Typography
          className="header-select-user-list"
          style={{ flex: 1, fontSize: 18, fontWeight: "bolder", color: "red" }}
        >
          Select User
        </Typography>
        <Search
          onSearch={onSearch}
          className="search-user-box"
          style={{ flex: 1 }}
        />
      </div>

      <Divider />
      <Table
        rowSelection={{
          type: "radio",
          defaultSelectedRowKeys: [props.DefaultUser],
          ...rowSelection,
        }}
        columns={columns}
        dataSource={data}
        rowKey={(record) => record.id}
        onRow={(record, rowIndex) => {
          return {
            onClick: () => {
              const { id: selectedRowKeys } = record;
              const selectedRows = record;
              setSelectedRowKeys([selectedRowKeys]);
              rowSelection.onChange([selectedRowKeys], [selectedRows]);
            },
          };
        }}
        pagination={pagination}
        loading={loading}
        onChange={onChange}
      />
    </div>
  );
};

export default ListUser;
