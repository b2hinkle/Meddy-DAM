namespace MeddyExplorerApp.Services
{
    /// <summary>
    /// Library of helpful functions for the MeddyExplorerState service.
    /// </summary>
    public static class MeddyExplorerStateExtensionMethods
    {
        /// <summary>
        /// "Pointer-select"
        /// </summary>
        public static void ExclusiveSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            meddyExplorerState.ClearSelectedFiles();
            AdditiveSelectFile(meddyExplorerState, inFile);
        }

        /// <summary>
        /// "Space-select"
        /// </summary>
        public static void AdditiveSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            if (meddyExplorerState.GetSelectedFiles().Contains(inFile) == false)
            {
                meddyExplorerState.AddToSelectedFiles(inFile);
            }
        }

        /// <summary>
        /// "Ctrl-select"
        /// </summary>
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

        /// <summary>
        /// "Shift-select"
        /// </summary>
        public static void ExclusiveRangeSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            if (meddyExplorerState.GetSelectedFiles().Any() == false)
            {
                // Our from index is currently dependent on selected files
                AdditiveSelectFile(meddyExplorerState, inFile); // just make sure that the file is selected
                return;
            }

            int fromIndex = meddyExplorerState.Files.IndexOf(meddyExplorerState.GetSelectedFiles().Last()); // TODO: use a more appropriate starting point
            int toIndex = meddyExplorerState.Files.IndexOf(inFile);

            meddyExplorerState.ClearSelectedFiles();
            AdditiveRangeSelectByIndexes(meddyExplorerState, fromIndex, toIndex);
        }

        /// <summary>
        /// "Ctrl-Shift-select"
        /// </summary>
        public static void AdditiveRangeSelectFile(this MeddyExplorerState meddyExplorerState, FileSystemInfo inFile)
        {
            if (meddyExplorerState.GetSelectedFiles().Any() == false)
            {
                // Our from index is currently dependent on selected files
                AdditiveSelectFile(meddyExplorerState, inFile); // just make sure that the file is selected
                return;
            }

            int fromIndex = meddyExplorerState.Files.IndexOf(meddyExplorerState.GetSelectedFiles().Last()); // TODO: use a more appropriate starting point
            int toIndex = meddyExplorerState.Files.IndexOf(inFile);

            AdditiveRangeSelectByIndexes(meddyExplorerState, fromIndex, toIndex);
        }

        /// <summary>
        /// Shared helper function.
        /// NOTE: Indexes are relative to the MeddyExplorerState.Files list.
        /// </summary>
        private static void AdditiveRangeSelectByIndexes(MeddyExplorerState meddyExplorerState, int inFromIndex, int inToIndex)
        {
            if (inFromIndex == inToIndex)
            {
                // Nothing to do
                AdditiveSelectFile(meddyExplorerState, meddyExplorerState.Files[inFromIndex]); // just make sure that the file is selected
                return;
            }

            int iterateDirection = Math.Sign(inToIndex - inFromIndex);
            for (int i = inFromIndex; (iterateDirection > 0) ? (i <= inToIndex) : (i >= inToIndex); i += iterateDirection)
            {
                AdditiveSelectFile(meddyExplorerState, meddyExplorerState.Files[i]);
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
