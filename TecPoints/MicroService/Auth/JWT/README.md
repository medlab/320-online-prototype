# 背景

JWT 是一种用于授权的明文数据规范，通过签名方式保证不被串改

本文尝试通过一个简单的NodeJS程序来演示：

1. JWT结构的生成+签名
2. JWT结构的验证和解码

# 示例程序

注意： 下面用到几个预定义量:

1. header和payload定义
1. 公钥
2. 私钥

```js
    //named by demo.mjs
    //Purpose:
    //    repeat the process of jwt.io
    //Reference:
    //  1. https://jwt.io/
    //  2. https://stackoverflow.com/questions/39239051/rs256-vs-hs256-whats-the-difference
    //  3. https://nodejs.org/api/crypto.html
    //  4. https://www.geeksforgeeks.org/node-js-crypto-sign-function/
    //  5. https://nodejs.org/api/buffer.html#buffer_buf_tostring_encoding_start_end

    import crypto from 'crypto'
    console.log('--begin-- refinfo')
    const header={
        "alg": "RS256",
        "typ": "JWT"
    }

    const payload={
        "sub": "1234567890",
        "name": "John Doe",
        "admin": true,
        "iat": 1516239022
    }

    const privkey=`-----BEGIN RSA PRIVATE KEY-----
    MIIEogIBAAKCAQEAnzyis1ZjfNB0bBgKFMSvvkTtwlvBsaJq7S5wA+kzeVOVpVWw
    kWdVha4s38XM/pa/yr47av7+z3VTmvDRyAHcaT92whREFpLv9cj5lTeJSibyr/Mr
    m/YtjCZVWgaOYIhwrXwKLqPr/11inWsAkfIytvHWTxZYEcXLgAXFuUuaS3uF9gEi
    NQwzGTU1v0FqkqTBr4B8nW3HCN47XUu0t8Y0e+lf4s4OxQawWD79J9/5d3Ry0vbV
    3Am1FtGJiJvOwRsIfVChDpYStTcHTCMqtvWbV6L11BWkpzGXSW4Hv43qa+GSYOD2
    QU68Mb59oSk2OB+BtOLpJofmbGEGgvmwyCI9MwIDAQABAoIBACiARq2wkltjtcjs
    kFvZ7w1JAORHbEufEO1Eu27zOIlqbgyAcAl7q+/1bip4Z/x1IVES84/yTaM8p0go
    amMhvgry/mS8vNi1BN2SAZEnb/7xSxbflb70bX9RHLJqKnp5GZe2jexw+wyXlwaM
    +bclUCrh9e1ltH7IvUrRrQnFJfh+is1fRon9Co9Li0GwoN0x0byrrngU8Ak3Y6D9
    D8GjQA4Elm94ST3izJv8iCOLSDBmzsPsXfcCUZfmTfZ5DbUDMbMxRnSo3nQeoKGC
    0Lj9FkWcfmLcpGlSXTO+Ww1L7EGq+PT3NtRae1FZPwjddQ1/4V905kyQFLamAA5Y
    lSpE2wkCgYEAy1OPLQcZt4NQnQzPz2SBJqQN2P5u3vXl+zNVKP8w4eBv0vWuJJF+
    hkGNnSxXQrTkvDOIUddSKOzHHgSg4nY6K02ecyT0PPm/UZvtRpWrnBjcEVtHEJNp
    bU9pLD5iZ0J9sbzPU/LxPmuAP2Bs8JmTn6aFRspFrP7W0s1Nmk2jsm0CgYEAyH0X
    +jpoqxj4efZfkUrg5GbSEhf+dZglf0tTOA5bVg8IYwtmNk/pniLG/zI7c+GlTc9B
    BwfMr59EzBq/eFMI7+LgXaVUsM/sS4Ry+yeK6SJx/otIMWtDfqxsLD8CPMCRvecC
    2Pip4uSgrl0MOebl9XKp57GoaUWRWRHqwV4Y6h8CgYAZhI4mh4qZtnhKjY4TKDjx
    QYufXSdLAi9v3FxmvchDwOgn4L+PRVdMwDNms2bsL0m5uPn104EzM6w1vzz1zwKz
    5pTpPI0OjgWN13Tq8+PKvm/4Ga2MjgOgPWQkslulO/oMcXbPwWC3hcRdr9tcQtn9
    Imf9n2spL/6EDFId+Hp/7QKBgAqlWdiXsWckdE1Fn91/NGHsc8syKvjjk1onDcw0
    NvVi5vcba9oGdElJX3e9mxqUKMrw7msJJv1MX8LWyMQC5L6YNYHDfbPF1q5L4i8j
    8mRex97UVokJQRRA452V2vCO6S5ETgpnad36de3MUxHgCOX3qL382Qx9/THVmbma
    3YfRAoGAUxL/Eu5yvMK8SAt/dJK6FedngcM3JEFNplmtLYVLWhkIlNRGDwkg3I5K
    y18Ae9n7dHVueyslrb6weq7dTkYDi3iOYRW8HRkIQh06wEdbxt0shTzAJvvCQfrB
    jg/3747WSsf/zBTcHihTRBdAv6OmdhV4/dD5YBfLAkLrd+mX7iE=
    -----END RSA PRIVATE KEY-----`


    const pubkey=`-----BEGIN PUBLIC KEY-----
    MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnzyis1ZjfNB0bBgKFMSv
    vkTtwlvBsaJq7S5wA+kzeVOVpVWwkWdVha4s38XM/pa/yr47av7+z3VTmvDRyAHc
    aT92whREFpLv9cj5lTeJSibyr/Mrm/YtjCZVWgaOYIhwrXwKLqPr/11inWsAkfIy
    tvHWTxZYEcXLgAXFuUuaS3uF9gEiNQwzGTU1v0FqkqTBr4B8nW3HCN47XUu0t8Y0
    e+lf4s4OxQawWD79J9/5d3Ry0vbV3Am1FtGJiJvOwRsIfVChDpYStTcHTCMqtvWb
    V6L11BWkpzGXSW4Hv43qa+GSYOD2QU68Mb59oSk2OB+BtOLpJofmbGEGgvmwyCI9
    MwIDAQAB
    -----END PUBLIC KEY-----`

    console.log({
        header:header,
        payload:payload,
        privkey:privkey,
        pubkey:pubkey
    })

    console.log('--end-- refinfo')


    console.log('--begin-- base64url')
    var base64 = {};

    base64.encode = function(unencoded) {
    return Buffer.from(unencoded || '').toString('base64');
    };

    base64.decode = function(encoded) {
    return Buffer.from(encoded || '', 'base64');
    };

    base64.urlEncode = function(unencoded) {
    var encoded = base64.encode(unencoded);
    return encoded.replace('+', '-').replace('/', '_').replace(/=+$/, '');
    };

    base64.urlDecode = function(encoded) {
    encoded = encoded.replace('-', '+').replace('_', '/');
    while (encoded.length % 4)
        encoded += '=';
    return base64.decode(encoded);
    };

    const base64urlencode_header=base64.urlEncode(JSON.stringify(header));
    const base64urlencode_payload=base64.urlEncode(JSON.stringify(payload));

    console.log({
        base64urlencode_header:base64urlencode_header,
        base64urlencode_payload:base64urlencode_payload
    })
    console.log('--end-- base64url')

    const header_body_base64urlencode_data=`${base64urlencode_header}.${base64urlencode_payload}`
    const signmsg_buffer = crypto.createSign('RSA-SHA256')
    .update(`${header_body_base64urlencode_data}`)
    .sign({
        key: privkey,
        //padding: crypto.constants.RSA_PKCS1_PSS_PADDING
    });
    const signmsg=base64.urlEncode(signmsg_buffer)

    console.log({base64_signmsg:signmsg, fullmsg:`${header_body_base64urlencode_data}.${signmsg}`})

    let verify_succ=crypto.verify('RSA-SHA256', Buffer.from(header_body_base64urlencode_data), pubkey, Buffer.from(signmsg_buffer))
    console.log({verify_succ:verify_succ})

    verify_succ=crypto.verify('RSA-SHA256', Buffer.from(header_body_base64urlencode_data), pubkey, 
            Buffer.from(base64.urlDecode(signmsg))
    )
    console.log({verify_succ:verify_succ})

```

# 参考
1. https://darutk.medium.com/oauth-2-0-client-authentication-4b5f929305d4
2. https://datatracker.ietf.org/doc/html/rfc7519
3. https://datatracker.ietf.org/doc/html/rfc7523
4. https://jwt.io/
