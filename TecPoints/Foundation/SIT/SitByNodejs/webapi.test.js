// import {describe, it} from 'jest';
// const describe, it = require('jest')
// # --begin-- Learn Chapter

describe('测试学习套件', ()=>{
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
// # --end-- Learn Chapter

// # --begin-- Work Chapter
// # --end-- Work Chapter
