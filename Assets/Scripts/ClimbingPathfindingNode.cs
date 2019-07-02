using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingPathfindingNode : MonoBehaviour {
    #region SerializeFields
    [SerializeField] List<ClimbingPathfindingNode> _adjacentNodes = new List<ClimbingPathfindingNode>();
    [SerializeField] bool _groundAdjacent = false;
    #endregion

    #region Events
    [System.Serializable] public sealed class AdjacentNodeAddedEvent : UltEvents.UltEvent<ClimbingPathfindingNode> { }
    public AdjacentNodeAddedEvent adjacentNodeAdded;

    [System.Serializable] public sealed class AdjacentNodeRemovedEvent : UltEvents.UltEvent<ClimbingPathfindingNode> { }
    public AdjacentNodeRemovedEvent adjacentNodeRemoved;

    [System.Serializable] public sealed class GroundAdjacentChangedEvent : UltEvents.UltEvent<bool> { }
    public GroundAdjacentChangedEvent groundAdjacentChanged;
    #endregion

    #region Getters and Setters
    public void AddAdjacentNode(ClimbingPathfindingNode newAdjacentNode) {
        if (!_adjacentNodes.Contains(newAdjacentNode)) {
            _adjacentNodes.Add(newAdjacentNode);
            adjacentNodeAdded.Invoke(newAdjacentNode);
        }
    }

    public void RemoveAdjacentNode(ClimbingPathfindingNode newAdjacentNode) {
        if (_adjacentNodes.Contains(newAdjacentNode)) {
            _adjacentNodes.Remove(newAdjacentNode);
            adjacentNodeRemoved.Invoke(newAdjacentNode);
        }
    }

    public void SetGroundAdjacent(bool groundAdjacent) {
        if (_groundAdjacent != groundAdjacent) {
            _groundAdjacent = groundAdjacent;
            groundAdjacentChanged.Invoke(_groundAdjacent);
        }
    }

    public ReadOnlyCollection<ClimbingPathfindingNode> GetAdjacentNodes(ClimbingPathfindingNode ignore = null) {
        if (HasNodeToIgnore(ignore)) {
            return GetObservedNodes(ignore).AsReadOnly();
        } else {
            return _adjacentNodes.AsReadOnly();
        }
    }
    #endregion

    private bool HasNodeToIgnore(ClimbingPathfindingNode ignore) {
        return ignore != null;
    }

    private List<ClimbingPathfindingNode> GetObservedNodes(ClimbingPathfindingNode ignore) {
        var adjacentNodes = new List<ClimbingPathfindingNode>();
        foreach (var node in _adjacentNodes) {
            if (node != ignore) {
                adjacentNodes.Add(node);
            }
        }
        return adjacentNodes;
    }
}
