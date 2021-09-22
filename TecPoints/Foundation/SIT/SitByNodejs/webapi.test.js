// import {describe, it} from 'jest';
// const describe, it = require('jest')
// # --begin-- Learn Chapter
const path=require('path')
const fs=require('fs/promises')
const assert=require('assert')
const child_process=require('child_process')
const axios=require('axios');

// # --begin-- Work Chapter
describe('WebApi测试套件', ()=>{

    beforeAll(()=>{
        console.log('beforeAll')
    })
    beforeEach(()=>{
        console.log('beforeEach')
    })
    afterEach(()=>{
        console.log('afterEach')
    })
    afterAll(()=>{
        console.log('afterAll')
    })

    it.each([
        ['zhangsan','Hello zhangsan'],
        ['lisi','Hello lisi'],
        ['wangmazi','Hello wangmazi']
        ])(' call sayhello to %s should get response %s', (name,response) => {
        //call webapi with name=name
        //expect response=response
    });
    
    let file_path_and_size_list=[['1.txt',1]]
    it.each(file_path_and_size_list)('%s file should get Size: %s', (file_path,size)=>{
    });
})

describe("Blazor Server测试套件",()=>{
    beforeAll(()=>{
        //启动服务
        //启动puppeter
    })
    beforeEach(()=>{
        console.log('beforeEach')
    })
    afterEach(()=>{
        console.log('afterEach')
    })
    afterAll(()=>{
        //关闭puppeter
        //关闭服务
    })

    it.each([
        ['zhangsan','Hello zhangsan'],
        ['lisi','Hello lisi'],
        ['wangmazi','Hello wangmazi']
        ])(' Say hello to %s should get response %s', (name,response) => {
        //TODO set input with #name
        //TODO get msg from #msg
        //TODO do assert
    });
})
// # --end-- Work Chapter
