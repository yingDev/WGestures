using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace General.Hierarchy
{
    /// <summary>
    /// a hierarchal collection of nodes.
    /// the generic parameter allows a specified object type to be stored as the value in each node.
    /// 
    /// various methods provide enumerations of the node's hierarchy (these are created by searching the 
    ///     entire hierarchy, not just particular leaves)
    /// eg:
    /// 
    /// GetTopLevel()
    /// GetChildren()
    /// GetSiblings()
    /// GetDescendants()
    /// 
    /// Implements IList so that the Tree can be used like a list.
    /// 
    /// 
    /// </summary>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class Tree<V> : IList<TreeNode<V>>
    {
        /// <summary>
        /// default, empty constructor.
        /// </summary>
        public Tree()
        {
        }

        #region Events

        public delegate void NodeEvent(Object sender, TreeNode<V> node);
        public event NodeEvent NodeAdded;
        public event NodeEvent NodeRemoved;

        #endregion

        #region Fields

        private List<TreeNode<V>> _nodes = new List<TreeNode<V>>();

        #endregion

        #region Hierarchy Enumerators

        /// <summary>
        /// gets every node in the list, in hierarchy order (parents first)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeNode<V>> GetAllNodesInHierarchyOrder()
        {
            foreach (var node in GetTopLevel())
            {
                yield return node;
                foreach (var descNode in GetDescendents(node))
                    yield return descNode;
            }
        }

        /// <summary>
        /// get the top-level nodes (those without parents)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeNode<V>> GetTopLevel()
        {
            return (from    n in _nodes
                    where   n.Parent == null
                    orderby n.Index
                    select  n);
        }

        /// <summary>
        /// actually calculates the children of a specified node, by searching the entire
        /// tree (not just the children stored at the node)
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<V>> GetChildren(TreeNode<V> parent)
        {
            return (from    n in _nodes
                    where   n.Parent == parent
                    orderby n.Index
                    select  n);
        }

        /// <summary>
        /// calculate the siblings of the current node by searching the entire tree.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<V>> GetSiblings(TreeNode<V> node)
        {
            return (from    n in _nodes
                    where   n.Parent == node.Parent && n != node
                    orderby n.Index
                    select  n);
        }

        /// <summary>
        /// calculate all the descendents of the current node in hierarchy (top-down) order.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<V>> GetDescendents(TreeNode<V> node)
        {
            // iterate the node's children:
            foreach (var child in GetChildren(node))
            {
                // return the node;
                yield return child;

                // recurse:
                foreach (var desc in GetDescendents(child))
                    yield return desc;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// access this tree-node as an IList.
        /// </summary>
        public IList<TreeNode<V>> NodesList { get { return this; } }

        #endregion

        #region TreeNode.Add

        /// <summary>
        /// add a tree-node with the specified parent.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public TreeNode<V> Add(V value, TreeNode<V> parent)
        {
            // create the node;
            TreeNode<V> node = new TreeNode<V>();
            
            // set it's parent and value.
            node.Value = value;
            node.Parent = parent;

            // add into the collection:
            Add(node);

            // return it.
            return node;
        }

        /// <summary>
        /// add a tree-node with the specified parent.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parentIndex"></param>
        /// <returns></returns>
        public TreeNode<V> Add(V value, int parentIndex)
        {
            if (parentIndex >= Count)
                throw new ArgumentOutOfRangeException("Parent Index is Invalid");

            // create the node:
            TreeNode<V> node = new TreeNode<V>();

            // set it's parent:
            node.Parent = _nodes[parentIndex];

            // set it's value:
            node.Value = value;

            // add to the collection:
            Add(node);

            // return the new node:
            return node;
        }

        /// <summary>
        /// add a new top-level node.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TreeNode<V> Add(V value)
        {
            // create the node:
            TreeNode<V> node = new TreeNode<V>();

            // set it's value:
            node.Value = value;

            // add to the collection:
            Add(node);

            // return the new node:
            return node;
        }

        #endregion

        #region IList<TreeNode<V>> Members

        public int IndexOf(TreeNode<V> item)
        {
            return _nodes.IndexOf(item);
        }

        public void Insert(int index, TreeNode<V> item)
        {
            _nodes.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _nodes.RemoveAt(index);
        }

        public TreeNode<V> this[int index]
        {
            get
            {
                return _nodes[index];
            }
            set
            {
                _nodes[index] = value;
            }
        }

        public TreeNode<V> this[V value]
        {
            get
            {

                var qry = (from n in _nodes where n.Value.Equals(value) select n);
                if (qry.Count() > 0)
                    return qry.First();
                else
                    return null;
            }
                    
        }

        #endregion

        #region ICollection<TreeNode<V>> Members

        /// <summary>
        /// the main add method for the Tree-Node.
        /// </summary>
        /// <param name="item"></param>
        public void Add(TreeNode<V> item)
        {
            if (!_nodes.Contains(item))
            {
                if (item.Parent != null)
                {
                    if (_nodes.Contains(item.Parent))
                    {
                        // add the node to the collection
                        _nodes.Add(item);

                        // set the owner:
                        item.SetOwner(this);

                        // raise node added.
                        if (NodeAdded != null)
                            NodeAdded(this, item);
                    }
                    else
                        throw new ApplicationException("Parent Node not in Collection!");
                }
                else
                {
                    // add the node to the collection
                    _nodes.Add(item);

                    // set the owner:
                    item.SetOwner(this);

                    // raise node added.
                    if (NodeAdded != null)
                        NodeAdded(this, item);
                }
            }
            else
            {
                if (_nodes.Contains(item.Parent))
                {
                    // find it's index location:
                    int index = _nodes.IndexOf(item);

                    // update the value to be sure:
                    _nodes[index] = item;

                    // set the owner:
                    item.SetOwner(this);

                    // raise node-added.
                    if (NodeAdded != null)
                        NodeAdded(this, item);
                }
                else
                    throw new ApplicationException("Parent Node not in Collection!");
            }

        }

        public void Clear()
        {
            _nodes.Clear();
        }

        public bool Contains(TreeNode<V> item)
        {
            return _nodes.Contains(item);
        }

        public bool Contains(V item)
        {
            // search the nodes for a matching v.
            var qry = (from n in _nodes
                       where n.Value.Equals(item)
                       select n);

            // return true if the results are > 0
            return qry.Count() > 0;

        }

        public void CopyTo(TreeNode<V>[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < _nodes.Count; i++)
            {
                array[i] = _nodes[i - arrayIndex];
            }
        }

        public int Count
        {
            get { return this._nodes.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TreeNode<V> item)
        {
            return _nodes.Remove(item);
        }

        #endregion

        #region IEnumerable<TreeNode<V>> Members

        public IEnumerator<TreeNode<V>> GetEnumerator()
        {
            foreach (var node in _nodes)
                yield return node;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var node in _nodes)
                yield return node;
        }

        #endregion

        /// <summary>
        /// print the tree's values 
        /// eg:
        /// TopLevel
        /// TopLevel\Level2_1
        /// TopLevel\Level2_2
        /// TopLevel\Level2_2\Level2_3\Sub\Value\Test
        /// TopLevel\Level2_3\Level3_3
        /// 
        /// </summary>
        /// <param name="writeToConsole"></param>
        /// <returns></returns>
        public String PrintTree(bool writeToConsole)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var node in GetAllNodesInHierarchyOrder())
            {
                sb.AppendLine(node.Path);
                if (writeToConsole)
                    Console.WriteLine(node.Path);
            }
            return sb.ToString();
        }

        /// <summary>
        /// string description of the item: (not a value)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "{TreeNode<" + typeof(V).Name + "> Count: " + Count + "}";
        }

    }
}
