import Mock from 'mockjs';

const mockdata = {
  'data|10': [
    {
      account: '@name',
      email: '@date',
      contact: false,
    },
  ],
};

Mock.mock('/listUsers', mockdata);
