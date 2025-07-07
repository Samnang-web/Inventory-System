import { defineStore } from "pinia";
import type { Category } from "../Types/CategoryInterface";
import { addCategories, deleteCategories, getCategories, updateCategories } from "../Services/CategoryService";


export const useCategoryStore = defineStore('category', {
    state: () => ({
        category: [] as Category[],
        loadingFetch: false,
        loadingCreate: false,
        loadingEdit: false,
        loadingRemove: false,
        errorFetch: null as string | null,
        errorCreate: null as string | null,
        errorEdit: null as string | null,
        errorRemove: null as string | null

    }),
    
    actions: {
        async fetchCategories (){
            this.loadingFetch = true;
            this.errorFetch = null;

            try{
                const response = await getCategories();
                this.category = response;
            }catch(err: any){
                this.errorFetch = err.message;
            }finally{
                this.loadingFetch = false
            }
        },

        async createCategories (data: Category){
            this.loadingCreate = true;
            this.errorCreate = null;

            try{
                const response = await addCategories(data);
                this.category.push(response);
            }catch(err: any){
                this.errorCreate = err.message;
            }finally{
                this.loadingCreate = false;
            }
        },

        async editCategories (id: number, data: Category){
            this.loadingEdit = true
            this.errorEdit = null;

            try{
                const response = await updateCategories(id, data);
                const index = this.category.findIndex(cat => cat.id === id);
                if (index !== -1) this.category[index] = response;
            }catch(err: any){
                this.errorEdit = err.message;
            }finally{
                this.loadingEdit = false
            }
        },

        async removeCategories (id: number){
            this.loadingRemove = true;
            this.errorRemove = null;

            try{
                await deleteCategories(id);
                this.category = this.category.filter(cat => cat.id !== id);
            }catch(err: any){
                this.errorRemove = err.message;
            }finally{
                this.loadingRemove = false;
            }
        }
    }
})