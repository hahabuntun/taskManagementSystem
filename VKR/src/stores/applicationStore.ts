import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { IWorker } from '../interfaces/IWorker';
import { DrawerEntityEnum } from '../enums/DrawerEntityEnum';

// Interface for the combined store
interface AppStore {
    // User Slice
    user: IWorker | null;
    setUser: (user: IWorker | null) => void;

    // Fields Slice
    fieldsToShow: Record<string, boolean>;
    setFieldsToShow: (fields: Record<string, boolean>) => void;

    // Drawer Slice
    isDrawerVisible: boolean;
    drawerEntityId: number | null;
    drawerEntityType: DrawerEntityEnum | null;
    showDrawer: (entityType: DrawerEntityEnum, entityId?: number) => void;
    hideDrawer: () => void;

    // Theme Slice
    isDarkMode: boolean;
    toggleTheme: () => void;
}

// Create the store with slices
const useApplicationStore = create<AppStore>()(
    persist(
        (set, _) => ({
            // User Slice
            user: null,
            setUser: (user: IWorker | null) => {
                if (user) {
                    set({ user });
                } else {
                    set({ user });
                    sessionStorage.removeItem('app-store');
                }
            },

            // Fields Slice
            fieldsToShow: {
                name: true,
                description: true,
                type: true,
                storyPoints: true,
                progress: true,
                createdAt: true,
                startDate: true,
                endDate: true,
                status: true,
                priority: true,
                project: true,
                sprint: true,
                creator: true,
                workers: true,
                viewers: true,
                responsible: true,
                tags: true
            },
            setFieldsToShow: (fields: Record<string, boolean>) => {
                const availableFields = [
                    "name", "description", "type", "storyPoints", "progress",
                    "createdAt", "startDate", "endDate", "status", "priority",
                    "project", "sprint", "creator", "workers", "viewers",
                    "responsible", "tags"
                ];

                if (fields.name === false) return;

                const invalidFields = Object.keys(fields).filter(
                    field => !availableFields.includes(field)
                );
                if (invalidFields.length > 0) return;

                set(state => ({
                    fieldsToShow: {
                        ...state.fieldsToShow,
                        ...fields
                    }
                }));
            },

            // Drawer Slice
            isDrawerVisible: false,
            drawerEntityId: null,
            drawerEntityType: null,
            showDrawer: (entityType: DrawerEntityEnum, entityId?: number) => {
                if (entityId) {
                    set({
                        isDrawerVisible: true,
                        drawerEntityId: entityId,
                        drawerEntityType: entityType
                    });
                }
            },
            hideDrawer: () => {
                set({
                    isDrawerVisible: false,
                    drawerEntityId: null,
                    drawerEntityType: null
                });
            },

            // Theme Slice
            isDarkMode: false,
            toggleTheme: () => set(state => ({ isDarkMode: !state.isDarkMode }))
        }),
        {
            name: 'app-store',
            storage: createJSONStorage(() => sessionStorage),
            partialize: (state: AppStore) => ({
                user: state.user,
                fieldsToShow: state.fieldsToShow,
                isDrawerVisible: state.isDrawerVisible,
                drawerEntityId: state.drawerEntityId,
                drawerEntityType: state.drawerEntityType,
                isDarkMode: state.isDarkMode
            })
        }
    )
);

export default useApplicationStore;