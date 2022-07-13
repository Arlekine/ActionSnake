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
        var leftDownCell = field[0, 0];
        var rightUpCell = field[field.Width - 1, field.Height - 1];

        var visibleFieldHeight = Mathf.Abs(field[0, 1].z - field[0, field.Height - 1].z) + _cameraZOffset * 2;
        
        Vector3 cameraPosition = new Vector3((leftDownCell.x + rightUpCell.x) / 2f, _camera.transform.position.y, (leftDownCell.z + rightUpCell.z) / 2f + _cameraZOffset);
        _camera.transform.position = cameraPosition;

        _camera.orthographicSize = visibleFieldHeight * 0.5f;
    }
}