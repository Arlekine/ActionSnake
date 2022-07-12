using UnityEngine;

public class FieldCameraPositioner
{
    private Camera _camera;
    private float _cameraZOffset;

    public FieldCameraPositioner(Camera camera, float cameraZOffset)
    {
        _camera = camera;
        _cameraZOffset = cameraZOffset;
    }

    public void PositionCamera(Field field)
    {
        var leftDownCell = field.AllCells[0][0];
        var rightUpCell = field.AllCells[field.Width - 1][field.Height - 1];

        var visibleFieldHeight = Mathf.Abs(field.AllCells[0][1].CenterWorldPosition.z - field.AllCells[0][field.Height - 1].CenterWorldPosition.z) + _cameraZOffset * 2;
        
        Vector3 cameraPosition = new Vector3((leftDownCell.CenterWorldPosition.x + rightUpCell.CenterWorldPosition.x) / 2f, _camera.transform.position.y, (leftDownCell.CenterWorldPosition.z + rightUpCell.CenterWorldPosition.z) / 2f + _cameraZOffset);
        _camera.transform.position = cameraPosition;

        _camera.orthographicSize = visibleFieldHeight * 0.5f;
    }
}