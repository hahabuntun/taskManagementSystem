// import { create } from 'zustand';
// import { persist, createJSONStorage } from 'zustand/middleware';
// import { DrawerEntityEnum } from '../enums/DrawerEntityEnum';

// // Interface for the DrawerStore
// export interface DrawerStore {
//     isDrawerVisible: boolean;
//     drawerEntityId: number | null;
//     drawerEntityType: DrawerEntityEnum | null;
//     showDrawer: (entityType: DrawerEntityEnum, entityId?: number) => void;
//     hideDrawer: () => void;
// }

// // Create the DrawerStore
// const useDrawerStore = create<DrawerStore>()(
//     persist(
//         (set) => ({
//             // Initial States
//             isDrawerVisible: false,
//             drawerEntityId: null,
//             drawerEntityType: null,

//             // Show Drawer
//             showDrawer: (entityType: DrawerEntityEnum, entityId?: number) => {
//                 if (entityId) {
//                     if (entityType === DrawerEntityEnum.WORKER) {
//                         set({ isDrawerVisible: true, drawerEntityId: entityId, drawerEntityType: entityType });
//                     } else {
//                         set({ isDrawerVisible: true, drawerEntityId: entityId, drawerEntityType: entityType });
//                     }
//                 }
//             },

//             // Hide Drawer
//             hideDrawer: () => {
//                 set({
//                     isDrawerVisible: false,
//                     drawerEntityId: null,
//                     drawerEntityType: null,
//                 });
//             },

//         }),
//         {
//             name: 'drawer-store',
//             storage: createJSONStorage(() => sessionStorage),
//             partialize: (state: DrawerStore) => ({
//                 isDrawerVisible: state.isDrawerVisible,
//                 drawerEntityId: state.drawerEntityId,
//                 drawerEntityType: state.drawerEntityType,
//             }),
//         }
//     )
// );

// export default useDrawerStore;