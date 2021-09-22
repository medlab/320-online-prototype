// import {describe, it} from 'jest';
// const describe, it = require('jest')
// # --begin-- Learn Chapter
const path=require('path')
const fs=require('fs/promises')
const assert=require('assert')
const child_process=require('child_process')
const axios=require('axios');

describe('测试学习套件--Jest框架学习', ()=>{
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
    it('should return hello world', ()=>{
        expect('hello world').toBe('hello world');
    });
    it.each([1,2,3,4,5,6,7,8,9,10])('should return %d', (num)=>{
        expect(num).toBe(num);
    })
})

describe('测试学习套件--相关API学习',()=>{
    describe('文件系统相关',()=>{
            it('learn how to  get list of file in test data dir',async ()=>{
                const files=await fs.readdir( path.join(path.dirname(__filename),'..','TestDatas'));
                files.length>0||assert.fail('no files')
            })
            it('learn how to get file size',async ()=>{
                const test_data_dir=path.join(path.dirname(__filename),'..','TestDatas')
                const files=await fs.readdir(test_data_dir);
                const file_stat=await fs.stat(`${test_data_dir}/${files[0]}`);
                file_stat.size>0||assert.fail('file should not be zero')
            })
        }
    )
    describe('进程相关',()=>{
        it('learn how to start&stop a process', async ()=>{
            // const ls = child_process.spawn('ls', ['-lh', '/usr']);
            // ls.stdout.on('data', (data) => {
            //     console.log(`stdout: ${data}`);
            // });
            //section run by sync and get result
            const result_str=child_process.execSync('ls -lh',{cwd:'.'});
            result_str.length>0||assert.fail('should not be empty')

            //run an not exist file and throw exception
            assert.throws(()=>{
                const result=child_process.execSync('lslslsl -lh',{cwd:'.'});    
            }, 'should throw an exception')

            //run multi process onebyone
            assert.doesNotThrow(()=>{
                const result=child_process.execSync('ls;ls;ls',
                    {
                        shell:true, 
                        encoding:'utf8'
                    })
                console.log(result)
            },'should run propertly')
        })
    })

    describe('puppeteer 相关', ()=>{
        it('learn how to use puppter', async ()=>{
            const puppeteer=require('puppeteer');
            //const browser=await puppeteer.launch({headless:false});
            const browser=await puppeteer.launch();
            const page=await browser.newPage();
            const response=await page.goto('https://httpbin.org/');
            response.ok||assert.fail('should be ok')
            //await page.screenshot({path:'./baidu.png'});
            await browser.close();
        })
    })

    describe('axios 相关',()=>{
        // it('learn how to basic use axios', async ()=>{   
        //     const result=await axios.get('https://www.debian.org/');
        //     //result.close()
        //     result.status===200||assert.fail('status should be 200')
        // })
        it('learn how to upload file', async ()=>{
            // const result=await axios.post('https://www.debian.org/',{
            //     file:fs.createReadStream('./baidu.png')
            // })
            // result.status===200||assert.fail('status should be 200')
        })

        it('learn how to access webapi by axios', async()=>{
            const result=await axios.get('https://httpbin.org/');
            result.status===200||assert.fail('status should be 200')
        })

    })

})
// # --end-- Learn Chapter