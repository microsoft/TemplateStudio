namespace Param_ItemNamespace.Services.Ink.UndoRedo
{
    public interface IUndoRedoOperation
    {
        void ExecuteUndo();

        void ExecuteRedo();
    }
}
