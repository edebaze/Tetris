using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public Vector3 rotationPoint;
    
    private double _currentFallTick;
    private float _fallTickCtr;     // current time before next fall

    private Game _gameMaster;
    private Spawner _spawner;
    
    private void Start() {
        _currentFallTick = Game.FallTick;
        _gameMaster = FindObjectOfType<Game>();
        _spawner = _gameMaster.GetComponent<Spawner>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateRotation();
        UpdateSwitch();
        UpdateFall();
    }

    #region UpdateMethods

    private void UpdateFall() {
        _fallTickCtr -= Time.deltaTime;
        if (_fallTickCtr < 0) {
            _fallTickCtr = (float)_currentFallTick;
            var movement = new Vector3(0, -1, 0);

            if (IsAllowedMovement(movement)) {
                transform.position += movement;
            }
            else {
                // add all block children to the grid
                _gameMaster.AddToGrid(transform);
                
                // disable movement script
                enabled = false;
            }
        }
    }

    private void UpdateMovement() {
        var movement = Vector3.zero;
        
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            movement = new Vector3(-1, 0, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            movement = new Vector3(1, 0, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            // if key is pressed, do one movement down instantly
            movement = new Vector3(0, -1, 0);
            
            // set current fall time to the player fall time
            _currentFallTick = Game.FallTick / 10;
            
            // reset the fallTickCtr to this current value
            _fallTickCtr = (float)_currentFallTick;
        }
        
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            // set current fall tick to default value
            _currentFallTick = Game.FallTick;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            _currentFallTick = 0;
            _fallTickCtr = 0;
        }

        if (movement != Vector3.zero && IsAllowedMovement(movement)) {
            _gameMaster.audioSourceEffects.PlayOneShot(_gameMaster.moveBlockAudio);
            transform.position += movement;
        }
    }

    private void UpdateRotation() {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow)) {
            Rotate();
            
            // revert rotation if current position is incorrect
            if (! IsAllowedMovement(new Vector3(0, 0, 0))) {
                Rotate(-1);
            }
        }
    }

    private void UpdateSwitch() {
        if (Input.GetKeyDown(KeyCode.A)) {
            _spawner.SwitchBlock(this);
            if (IsAllowedMovement(Vector3.zero)) {
                _spawner.SwitchBlock(this);
            }
        }
    }

    #endregion

    public void Rotate(int rotationFactor = 1) {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), rotationFactor * 90);
    }
    
    private bool IsAllowedMovement(Vector3 movement) {
        foreach (Transform child in transform) {
            (var x, var y) = Game.GetGridPosition(child.position);
            x += (int)movement.x;
            y += (int)movement.y;

            if (x is < 0 or >= Game.BackgroundWidth) {
                return false;
            }
        
            // check if object has hit the bottom
            if (y < 0) {
                return false;
            }

            // check if block hits other block
            if (y < Game.BackgroundHeight && Game.Grid[x, y] != null) {
                return false;
            }
        }

        return true;
    }
}
