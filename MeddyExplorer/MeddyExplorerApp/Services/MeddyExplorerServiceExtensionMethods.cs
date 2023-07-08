namespace MeddyExplorerApp.Services
{
    /// <summary>
    /// Library of helpful functions for the MeddyExplorerService service.
    /// </summary>
    public static class MeddyExplorerServiceExtensionMethods
    {
        /// <summary>
        /// "Pointer-select"
        /// </summary>
        public static void ExclusiveSelectFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            meddyExplorerService.ClearSelectedFiles();
            AdditiveSelectFile(meddyExplorerService, inFile);
        }

        /// <summary>
        /// "Space-select"
        /// </summary>
        public static void AdditiveSelectFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            if (meddyExplorerService.GetSelectedFiles().Contains(inFile) == false)
            {
                meddyExplorerService.AddToSelectedFiles(inFile);
            }
        }

        /// <summary>
        /// "Ctrl-select"
        /// </summary>
        public static void AdditiveToggleSelectFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            if (meddyExplorerService.GetSelectedFiles().Contains(inFile) == false)
            {
                meddyExplorerService.AddToSelectedFiles(inFile);
            }
            else
            {
                meddyExplorerService.RemoveFromSelectedFiles(inFile);
            }
        }

        /// <summary>
        /// "Shift-select"
        /// </summary>
        public static void ExclusiveRangeSelectFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            if (meddyExplorerService.GetSelectedFiles().Any() == false)
            {
                // Our from index is currently dependent on selected files
                AdditiveSelectFile(meddyExplorerService, inFile); // just make sure that the file is selected
                return;
            }

            int fromIndex = meddyExplorerService.Files.IndexOf(meddyExplorerService.GetSelectedFiles().Last()); // TODO: use a more appropriate starting point
            int toIndex = meddyExplorerService.Files.IndexOf(inFile);

            meddyExplorerService.ClearSelectedFiles();
            AdditiveRangeSelectByIndexes(meddyExplorerService, fromIndex, toIndex);
        }

        /// <summary>
        /// "Ctrl-Shift-select"
        /// </summary>
        public static void AdditiveRangeSelectFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            if (meddyExplorerService.GetSelectedFiles().Any() == false)
            {
                // Our from index is currently dependent on selected files
                AdditiveSelectFile(meddyExplorerService, inFile); // just make sure that the file is selected
                return;
            }

            int fromIndex = meddyExplorerService.Files.IndexOf(meddyExplorerService.GetSelectedFiles().Last()); // TODO: use a more appropriate starting point
            int toIndex = meddyExplorerService.Files.IndexOf(inFile);

            AdditiveRangeSelectByIndexes(meddyExplorerService, fromIndex, toIndex);
        }

        /// <summary>
        /// Shared helper function.
        /// NOTE: Indexes are relative to the MeddyExplorerService.Files list.
        /// </summary>
        private static void AdditiveRangeSelectByIndexes(MeddyExplorerService meddyExplorerService, int inFromIndex, int inToIndex)
        {
            if (inFromIndex == inToIndex)
            {
                // Nothing to do
                AdditiveSelectFile(meddyExplorerService, meddyExplorerService.Files[inFromIndex]); // just make sure that the file is selected
                return;
            }

            int iterateDirection = Math.Sign(inToIndex - inFromIndex);
            for (int i = inFromIndex; (iterateDirection > 0) ? (i <= inToIndex) : (i >= inToIndex); i += iterateDirection)
            {
                AdditiveSelectFile(meddyExplorerService, meddyExplorerService.Files[i]);
            }
        }

        public static void SelectNoneFiles(this MeddyExplorerService meddyExplorerService)
        {
            meddyExplorerService.ClearSelectedFiles();
        }

        public static void TargetFile(this MeddyExplorerService meddyExplorerService, FileSystemInfo inFile)
        {
            meddyExplorerService.TargetedFile = inFile;
        }
    }
}
