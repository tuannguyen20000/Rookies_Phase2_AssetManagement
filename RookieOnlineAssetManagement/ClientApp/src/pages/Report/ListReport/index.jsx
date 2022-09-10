import React, { useEffect, useState } from "react";
import axiosInstance from "../../../axios";
import { Table, Typography, Button } from "antd";
import "antd/dist/antd.css";
import reportService from "../../../services/reportService";

const ReportTable = () => {
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
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
        `reports/getlist?&page=${params.pagination.current}&pageSize=${params.pagination.pageSize}&sortOrder=${params.sortOrder}&sortField=${params.sortField}`
      )
      .then((results) => {
        setData(results.reports);
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
      title: "Category",
      dataIndex: "category",
      ellipsis: true,
      width: "15%",
      sorter: true,
    },
    {
      title: "Total",
      dataIndex: "total",
      width: "13%",
      sorter: true,
    },
    {
      title: "Assigned",
      dataIndex: "assigned",
      width: "13%",
      sorter: true,
    },
    {
      title: "Available",
      dataIndex: "available",
      width: "13%",
      sorter: true,
    },
    {
      title: "Not Available",
      dataIndex: "notAvailable",
      width: "13%",
      sorter: true,
    },
    {
      title: "Waiting For Recycling",
      dataIndex: "waitingForRecycling",
      width: "15%",
      sorter: true,
    },
    {
      title: "Recycled",
      dataIndex: "recycled",
      width: "13%",
      sorter: true,
    },
  ];

  const onChange = (newPagination, filters, sorter, extra) => {
    fetchData({
      sortField: sorter.field,
      sortOrder: sorter.order,
      pagination: newPagination,
      ...filters,
    });
  };

  const ExportToExcel = () => {
    reportService.exportToExcel();
  };

  return (
    <div className="user-table">
      <Typography className="header-user-list">Report</Typography>
      <Button
        className="create-user-button"
        style={{
          marginBottom: "10px",
        }}
        type="primary"
        onClick={ExportToExcel}
      >
        Export
      </Button>

      <Table
        columns={columns}
        dataSource={data}
        rowKey={(record) => record.id}
        pagination={pagination}
        loading={loading}
        onChange={onChange}
      />
    </div>
  );
};

export default ReportTable;
