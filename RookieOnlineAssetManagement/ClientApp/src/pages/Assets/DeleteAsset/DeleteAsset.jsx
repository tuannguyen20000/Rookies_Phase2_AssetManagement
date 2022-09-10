import React, { useEffect, useState } from "react";

import Modal from "antd/lib/modal/Modal";
import assetService from "../../../services/assetService";

const DeleteAsset = (props) => {
  const [modal, setModal] = useState(props.bool);
  const updateUrl = `/assets/update/${props.id}`;
  const [checkHisory, setCheckHistory] = useState();

  useEffect(() => {
    setModal(props.bool);
    if (props.id != null) {
      assetService.checkHistory(props.id).then((response) => {
        if (response.data === false) {
          setCheckHistory(false);
        } else {
          setCheckHistory(true);
        }
      });
    }
  }, [props.bool]);

  const handleDeleteAssset = () => {
    assetService.deleteAsset(props.id).then((response) => {
      if (response.data === true) {
        props.handleCancel();
        window.location.reload();
      }
    });
  };
  return (
    <div>
      {!checkHisory ? (
        <Modal
          className="modalStyle"
          title="Are you sure"
          visible={modal}
          onCancel={props.handleCancel}
          footer={null}
        >
          <div>
            <p className="modal-body">Do you want to delete this asset?</p>
            <div className="modal-button-group-container">
              <div className="modal-disable-button-container">
                <button className="btn" onClick={handleDeleteAssset}>
                  Delete
                </button>
              </div>
              <div className="modal-cancel-button-container">
                <button className="btn" onClick={props.handleCancel}>
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </Modal>
      ) : (
        <Modal
          className="modalStyle"
          title="Cannot Delete Asset"
          visible={modal}
          onCancel={props.handleCancel}
          footer={null}
        >
          <div>
            <p>
              Cannot delete the asset bescause it belongs to one or more
              historical assignments.
            </p>
            <p>
              If the asset is not able to be used anymore, please update its
              this state in &nbsp;
              <a href={updateUrl}>Edit Asset page</a>
            </p>
          </div>
        </Modal>
      )}
    </div>
  );
};

export default DeleteAsset;
