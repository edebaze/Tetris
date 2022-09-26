using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public GameObject[] TetrisForms;
    private Vector3 _spawnPosition;

    // QUEUE : stack of next 
    public Transform queueDisplayer;                        // position where the queue is displayed
    [SerializeField] private int _queueLength = 5;          // number of objects to display in the queue
    private List<BlockMovement> _queue = new ();            // list of queued blocks
    private const int QueueOffset = -3;                     // offset between each blocks in display

    // SWITCH BLOCK : block to switch with current blocl
    public Transform switchBlockDisplayer;   // position where the switch block is displayed
    private BlockMovement _switchBlock;       // block that can be swap with current 
    
    public Vector3 SpawnPosition {
        set => _spawnPosition = value;
    }

    public void Init() {
        SetQueue();
        Spawn();
    }

    public void Reset() {
        foreach (var block in _queue) {
            Destroy(block);
        }
        _queue = new List<BlockMovement>();
        _switchBlock = null;
    }

    // =================================================================================================================
    #region Spawn

    public void Spawn() {
        // display queue
        var i = 0;
        foreach (var block in _queue) {
            if (i == 0) {
                // spawn first element of the queue
                var copy = Instantiate(block, _spawnPosition, Quaternion.identity);
                copy.GetComponent<BlockMovement>().enabled = true;
                
                // destroy block as child of the queueDisplayer object
                Destroy(block.gameObject);
            }
            else {
                // move all elements by 1 offset
                block.transform.position -= new Vector3(0, QueueOffset, 0);
            }
            
            i++;
        }

        // remove first element of the list
        _queue.RemoveAt(0);
        
        // add new block at the end of the queue
        AddRandomBlockInQueue();
    }
    
    private GameObject GetRandomBlock() {
        return TetrisForms[Random.Range(0, TetrisForms.Length)];
    }

    #endregion
    
    // =================================================================================================================
    #region Switch Block

    public void SwitchBlock(BlockMovement block) {
        if (_switchBlock == null) {
            SetAsSwitchBlock(block);
            Spawn();
            return;
        }

        _switchBlock.transform.position = block.transform.position;
        _switchBlock.enabled = true;
        SetAsSwitchBlock(block);
    }

    private void SetAsSwitchBlock(BlockMovement block) {
        // reverse rotation
        block.transform.rotation = Quaternion.identity;
        
        // disable block and set it in switch block position
        block.enabled = false;
        var blockPosition = switchBlockDisplayer.transform.position;
        blockPosition.x -= switchBlockDisplayer.transform.localScale.x / 2;
        block.transform.position = blockPosition;
        _switchBlock = block;
    }

    #endregion
    
    // =================================================================================================================
    #region Queue

    void SetQueue() {
        for (var i = 0; i < _queueLength; i++) {
            AddRandomBlockInQueue();
        }
    }
    
    private void AddRandomBlockInQueue() {
        var index = _queue.Count;
        
        // instantiate block at queueDisplayer position
        var pos = queueDisplayer.position;
        pos.y += QueueOffset * index;
        _queue.Add(
            Instantiate(
                GetRandomBlock(),  
                pos, 
                Quaternion.identity
            ).GetComponent<BlockMovement>()
        );
        
    }

    #endregion
    



}
