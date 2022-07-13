using UnityEngine;

public class StylePointsPillSpawner : FieldObjectSpawner<StylePointsPill>
{
    [Space] 
    [SerializeField] private StylePointsCounter _stylePointsCounter;
    
    protected override void InitializeObject(StylePointsPill fieldObject)
    {
        fieldObject.Init(_stylePointsCounter);
    }
}