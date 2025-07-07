import { defineStore } from "pinia";
import type { Product } from "../Types/ProductInterface";
import { addProduct, deleteProduct, getProduct, updateProduct } from "../Services/ProductService";


export const useProductStore = defineStore('product',{
    state: () => ({
        products: [] as Product[],
        loadingFetch: false,
        loadingCreate: false,
        loadingEdit: false,
        loadingDelete: false,
        errorFetch: null as string | null,
        errorCreate: null as string | null,
        errorEdit: null as string | null,
        errorDelete: null as string | null,
    }),

    actions: {
        async fetchProduct(){
            this.loadingFetch = true;
            this.errorFetch = null
            try{
                const response = await getProduct();
                this.products = response;
            }catch(err: any){
                this.errorFetch = err.message;
            }finally{
                this.loadingFetch = false
            }
        },

        async createProduct (formdata: FormData){
            this.loadingCreate = true;
            this.errorCreate = null;
            try{
                const response = await addProduct(formdata);
                this.products.push(response);
            }catch(err: any){
                this.errorCreate = err.message;
            }finally{
                this.loadingCreate = false
            }
        },

        async editProduct (id: number, formdata: FormData){
            this.loadingEdit = true
            this.errorEdit = null
            try{
                const response = await updateProduct(id, formdata);
                const index = this.products.findIndex(product => product.id === id);
                if (index !== -1) this.products[index] = response;
            }catch(error: any){
                this.errorEdit = error.message;
            }finally{
                this.loadingEdit = false
            }
        },

        async removeProduct(id: number){
            this.loadingDelete = true
            this.errorDelete = null
            try{
                await deleteProduct(id);
                this.products = this.products.filter(p => p.id !== id);
            }catch(err: any){
                this.errorDelete = err.message
            }finally{
                this.loadingDelete = false
            }
        }
    }
})