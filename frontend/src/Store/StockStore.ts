import { defineStore } from "pinia";
import type { Stock, StockFormDto } from "../Types/StockInterface";
import { addStock, deleteStock, getStock, updateStock } from "../Services/StockService";


export const useStockStore = defineStore('stock', {
    state: () => ({
        stock: [] as Stock[],
        loadingFetch: false,
        loadingCreate: false,
        loadingEdit: false,
        loadingRemove: false,
        errorFetch: null as string | null,
        errorCreate: null as string | null,
        errorEdit: null as string | null,
        errorRemove: null as string | null,
    }),

    actions: {
        async fetchStock(){
            this.loadingFetch = true;
            this.errorFetch = null;

            try{
                const response = await getStock();
                this.stock = response;
            }catch(err: any){
                this.errorFetch = err.message;
            }finally{
                this.loadingFetch = false;
            }
        },

        async createStock(data: StockFormDto){
            this.loadingCreate = true;
            this.errorCreate = null;

            try{
                const response = await addStock(data);
                this.stock.push(response);
            }catch(err: any){
                this.errorCreate = err.message;
            }finally{
                this.loadingCreate = false;
            }
        },

        async editStock(id: number, data: StockFormDto){
            this.loadingEdit = true;
            this.errorEdit = null;

            try{
                const response = await updateStock(id, data);
                const index = this.stock.findIndex(stocks => stocks.id === id);
                if (index !== -1) this.stock[index] = response;
            }catch(err: any){
                this.errorEdit = err.message;
            }finally{
                this.loadingEdit = false;
            }
        },

        async removeStock(id: number){
            this.loadingRemove = true;
            this.errorRemove = null;

            try{
                await deleteStock(id);
                this.stock = this.stock.filter(stocks => stocks.id !== id);

            }catch(err: any){
                this.errorRemove = err.message;
            }finally{
                this.loadingRemove = false;
            }
        },
    }
})