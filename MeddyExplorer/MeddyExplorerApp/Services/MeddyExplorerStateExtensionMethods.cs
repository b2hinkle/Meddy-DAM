namespace MeddyExplorerApp.Services
{
    /// <summary>
    /// Library of helpful functions for the MeddyExplorerState service.
    /// </summary>
    public static class MeddyExplorerStateExtensionMethods
    {
        public static void ExclusiveSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            meddyExplorerState.ClearSelectedFiles();
            meddyExplorerState.AddToSelectedFiles(inFile);
        }

        public static void AdditiveSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            meddyExplorerState.AddToSelectedFiles(inFile);
        }

        public static void AdditiveToggleSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            if (meddyExplorerState.GetSelectedFiles().Contains(inFile) == false)
            {
                meddyExplorerState.AddToSelectedFiles(inFile);
            }
            else
            {
                meddyExplorerState.RemoveFromSelectedFiles(inFile);
            }
        }

        public static void SelectNoneFiles(this MeddyExplorerState meddyExplorerState)
        {
            meddyExplorerState.ClearSelectedFiles();
        }

        public static void TargetFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            meddyExplorerState.TargetedFile = inFile;
        }
    }
}
