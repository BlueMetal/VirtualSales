using System;
using System.Drawing;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using VirtualSales.iOS.DraggableList;
using System.Collections.Generic;
using VirtualSales.Core.ViewModels.Tools;
using ReactiveUI;
using System.Reactive.Linq;
using VirtualSales.Core.ViewModels;
using System.Windows.Input;

namespace VirtualSales.iOS.ViewControllers
{
    

    public partial class SelectedToolsViewController : UICollectionViewController, IDraggableDataSource
    {
        public class SelectedToolViewModel : ReactiveObject
        {
            private bool _isEmpty;

            public ICommand ShowAvailableToolsCommand { get; private set; }
            public ICommand SetActiveToolCommand { get; private set; }
            public IReactiveList<IToolViewModel> SelectedTools { get; private set; }

            public bool IsEmpty
            {
                get { return _isEmpty; }
                set { this.RaiseAndSetIfChanged(ref _isEmpty, value); }
            }

            public SelectedToolViewModel(UIView toolboxView, AvailableToolsContainerViewController container, MeetingViewModel meeting, IReactiveList<IToolViewModel> selectedTools, NavigationPaneViewModel navigationPane)
            {
                SelectedTools = selectedTools;

                IsEmpty = SelectedTools.Count == 0;
                SelectedTools.Changed.ObserveOn(RxApp.MainThreadScheduler)
                             .Subscribe(p =>
                                        {
                                            IsEmpty = SelectedTools.Count == 0;
                                        });

                var showAvailableToolsCommand = new ReactiveCommand();
                showAvailableToolsCommand.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ =>
                {
                    UIView.BeginAnimations(null);
                    UIView.SetAnimationDuration(0.25);
                    toolboxView.Alpha = 1.0f;
                    UIView.CommitAnimations();
                });

                container.CloseToolboxRequested += (s, e) =>
                                                   {
                                                       UIView.BeginAnimations(null);
                                                       UIView.SetAnimationDuration(0.25);
                                                       toolboxView.Alpha = 0.0f;
                                                       UIView.CommitAnimations();
                                                   };
                ShowAvailableToolsCommand = showAvailableToolsCommand;
                navigationPane.ShowToolLibraryAction = () => ShowAvailableToolsCommand.Execute(null);

                var setActiveToolsCommand = new ReactiveCommand();
                setActiveToolsCommand.ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(tool => meeting.ActiveTool = (IToolViewModel)tool);
                SetActiveToolCommand = setActiveToolsCommand;
            }

        }
        private SelectedToolViewModel _viewModel;
        public SelectedToolViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                _viewModel = value;
                HookupViewModel();
            }
        }

        internal DraggableListTargetFlowLayout DraggableLayout
        {
            get { return (DraggableListTargetFlowLayout)Layout; }
        }

        private void HookupViewModel()
        {
            CollectionView.ReloadData();
            ViewModel.WhenAnyValue(p => p.IsEmpty, p => p)
                     .ObserveOn(RxApp.MainThreadScheduler)
                     .Subscribe(p =>
                                {
                                    dragItemsIntoHereLabel.Hidden = !p;
                                });
        }

        public SelectedToolsViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DraggableLayout.DataSource = this;
            DraggableLayout.SetupCollectionView();

            CollectionView.AllowsSelection = true;
            CollectionView.AllowsMultipleSelection = false;

            CollectionView.RegisterNibForCell(ToolCell.Nib, ToolCell.Key);
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var tool = ViewModel.SelectedTools[indexPath.Item];
            ViewModel.SetActiveToolCommand.Execute(tool);
        }

        public override int NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override int GetItemsCount(UICollectionView collectionView, int section)
        {
            if (ViewModel == null) return 0;
            return ViewModel.SelectedTools.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(ToolCell.Key, indexPath) as ToolCell;
            cell.Text = ViewModel.SelectedTools[indexPath.Item].Name;
            cell.SetBorder();
            return cell;
        }

        #region IDraggableDataSource Implementation
        public void ItemWillMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath)
        {
            if (ViewModel == null) return;

            var item = ViewModel.SelectedTools[fromIndexPath.Item];
            ViewModel.SelectedTools.RemoveAt(fromIndexPath.Item);
            ViewModel.SelectedTools.Insert(toIndexPath.Item, item);
        }

        public void ItemDidMoveToIndexPath(NSIndexPath fromIndexPath, NSIndexPath toIndexPath)
        {
        }

        public object ItemAtIndexPath(NSIndexPath path)
        {
            if (ViewModel == null) return null;

            return ViewModel.SelectedTools[path.Item];
        }

        public void ItemWillRemoveFromIndexPath(NSIndexPath path)
        {
            if (ViewModel == null) return;

            ViewModel.SelectedTools.RemoveAt(path.Item);
        }

        public void ItemDidRemoveFromIndexPath(NSIndexPath path)
        {
        }

        public void ItemWillAddToIndexPath(NSIndexPath path, object data)
        {
            if (ViewModel == null) return;

            var item = data as IToolViewModel;
            if (item != null)
            {
                ViewModel.SelectedTools.Insert(path.Item, item);
            }
        }

        public void ItemDidAddToIndexPath(NSIndexPath path, object data)
        {

        }

        public bool CanMoveItemAtIndexPath(NSIndexPath path)
        {
            return true;
        }
        #endregion
    }
}