import { defineStore } from "pinia";
import type { User } from "../Types/UserInterface";
import { getUser } from "../Services/UserService";


export const useUserStore = defineStore('user', {
    state: () => ({
        users: [] as User[],
        currentUser: null as User | null,
        loadingFetch: false,
        errorFetch: null as string | null,
    }),

    actions: {
        async fetchUser(){
            this.loadingFetch = true;
            this.errorFetch = null;

            try{
                const response = await getUser();
                this.users = response;

                this.currentUser = response[0] || null;
            }catch(err: any){
                this.errorFetch = err.message;
            }finally{
                this.loadingFetch = false;
            }
        },
    }
})