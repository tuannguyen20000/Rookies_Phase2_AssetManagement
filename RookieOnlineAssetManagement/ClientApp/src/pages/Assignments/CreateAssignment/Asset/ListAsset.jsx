import React, { useEffect, useState } from "react";
import axiosInstance from "../../../../axios";
import { Table, Typography, Input, Divider } from "antd";

import "antd/dist/antd.css";

const ListAsset = (props) => {
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);

  const rowSelection = {
    selectedRowKeys: selectedRowKeys,
    onChange: (selectedRowKeys, selectedRows) => {
      setSelectedRowKeys(selectedRowKeys);
      var id = selectedRowKeys[0];
      var assetName = selectedRows[0].assetName;
      var infoAsset = {
        id: id,
        assetName: assetName,
      };
      props.onSelectedAsset(infoAsset);
    },
  };

  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    keyword: "",
  });
  const fetchData = (params = {}) => {
    setLoading(true);
    axiosInstance
      .get(
        `assets/GetList?&page=${params.pagination.current}&pageSize=${params.pagination.pageSize}&keyword=${params.pagination.keyword}&sortOrder=${params.sortOrder}&sortField=${params.sortField}`
      )
      .then((results) => {
        let dataAsset = [];
        results.assets.map((asset) => {
          dataAsset.push({
            id: asset.id,
            assetName: asset.assetName,
            assetCode: asset.assetCode,
            categoryName: asset.category,
          });
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
      width: "20%",
      sorter: true,
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      ellipsis: true,
      defaultSortOrder: "ascend",
      sorter: true,
    },
    {
      title: "Category Name",
      dataIndex: "categoryName",
      ellipsis: true,
      width: "30%",
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
          Select Asset
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
          defaultSelectedRowKeys: [props.DefaultAsset],
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

export default ListAsset;
