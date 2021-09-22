// import {describe, it} from 'jest';
// const describe, it = require('jest')
// # --begin-- Learn Chapter
const path=require('path')
const fs=require('fs/promises')
const assert=require('assert')
const child_process=require('child_process')
const axios=require('axios');

const base_url='http://localhost:5100'

const delay = ms => new Promise(resolve => setTimeout(resolve, ms))

// # --begin-- Work Chapter
describe('WebApi测试套件', ()=>{

    let dotnet_process;
    
    beforeAll(async ()=>{
        console.log('beforeAll')
        const webapi_project_dir=path.join(path.dirname(__filename),'..','WebApi')
        dotnet_process=child_process.spawn(
            `dotnet run --urls ${base_url}`, {
            cwd:webapi_project_dir,
            shell:true,
        })
        // await new Promise((resolve, reject)=>{
        //     dotnet_process.on('spawn', (code)=>{
        //         console.log('dotnet_process.on spawn', code)
        //         resolve()
        //     })
        // });
        //TODO too bad here, try to find some more stable code
        await delay(2000)
    })
    beforeEach(()=>{
        console.log('beforeEach')
    })
    afterEach(()=>{
        console.log('afterEach')
    })
    afterAll(async ()=>{
        console.log('afterAll')
        //TODO find a safe way to do it
        //dotnet_process.isRunning()&&
        dotnet_process.kill();
        // await new Promise((resolve, reject)=>{
        //     dotnet_process.on('killed', (code)=>{
        //         console.log('dotnet_process.on killed', code)
        //         resolve()
        //     })
        // });
        await delay(2000);
    })

    it.each([
        ['zhangsan','Hello zhangsan'],
        ['lisi','Hello lisi'],
        ['wangmazi','Hello wangmazi']
        ])(' call sayhello to %s should get response %s', async (name, response) => {
            const result=await axios.get(`${base_url}/HelloApi/SayHello?name=${name}`);
            console.log(`response is ${result.data}`)
            assert.equal(result.data,response)
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
